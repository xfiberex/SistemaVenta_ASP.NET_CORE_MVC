using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private static readonly HttpClient _httpClient = new();
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IFireBaseService _firebaseService;
        private readonly IUtilidadesService _utilidadesService;
        private readonly ICorreoService _correoService;

        public UsuarioService(
            IGenericRepository<Usuario> repositorio,
            IFireBaseService firebaseService,
            IUtilidadesService utilidadesService,
            ICorreoService correoService
            )
        {
            _repositorio = repositorio;
            _firebaseService = firebaseService;
            _utilidadesService = utilidadesService;
            _correoService = correoService;
        }

        private static async Task<string> ObtenerHtmlPlantillaCorreoAsync(string urlPlantillaCorreo)
        {
            if (string.IsNullOrWhiteSpace(urlPlantillaCorreo))
            {
                return string.Empty;
            }

            using HttpResponseMessage response = await _httpClient.GetAsync(urlPlantillaCorreo);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        /* Este método encapsula la lógica para consultar y devolver una lista de usuarios 
         * junto con la información de sus roles asociados de manera asíncrona */
        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repositorio.Consultar();
            return query.Include(r => r.IdRolNavigation).ToList();
        }

        /* Este método encapsula la lógica para crear un nuevo usuario en el sistema, manejar la subida de una imagen (si se proporciona), 
         * enviar un correo electrónico de confirmación (si se proporciona una plantilla de correo electrónico) y devolver el usuario recién 
         * creado con la información de su rol asociado. */
        public async Task<Usuario> Crear(Usuario entidad, Stream? Foto = null, string NombreFoto = "", string UrlPlantillaCorreo = "")
        {
            Usuario? usuario_Existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo);

            if (usuario_Existe != null)
            {
                throw new TaskCanceledException("El correo ya existe");
            }

            try
            {
                string clave_generada = _utilidadesService.GenerarClave();
                entidad.Clave = _utilidadesService.HashearClave(clave_generada);
                entidad.NombreFoto = NombreFoto;

                if (Foto != null)
                {
                    string urlFoto = await _firebaseService.SubirStorege(Foto, "carpeta_usuario", NombreFoto);
                    entidad.UrlFoto = urlFoto;
                }

                Usuario usuario_creado = await _repositorio.Crear(entidad);

                if (usuario_creado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario");
                }

                if (!string.IsNullOrEmpty(UrlPlantillaCorreo))
                {
                    UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[correo]", usuario_creado.Correo).Replace("[clave]", clave_generada);
                    string htmlCorreo = await ObtenerHtmlPlantillaCorreoAsync(UrlPlantillaCorreo);

                    if (!string.IsNullOrEmpty(htmlCorreo))
                    {
                        await _correoService.EnviarCorreo(usuario_creado.Correo, "Cuenta Creada", htmlCorreo);
                    }
                }

                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario == usuario_creado.IdUsuario);
                usuario_creado = query.Include(r => r.IdRolNavigation).First();
                return usuario_creado;
            }
            catch
            {
                throw;
            }
        }

        /*  Este método encapsula la lógica para editar la información de un usuario en el sistema, 
         *  incluyendo la posibilidad de actualizar su imagen de perfil, y devuelve el usuario editado 
         *  con la información de su rol asociado. */
        public async Task<Usuario> Editar(Usuario entidad, Stream? Foto = null, string NombreFoto = "")
        {
            Usuario? usuario_Existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo && u.IdUsuario != entidad.IdUsuario);

            if (usuario_Existe != null)
            {
                throw new TaskCanceledException("El correo ya existe");
            }

            try
            {
                IQueryable<Usuario> queryUsuario = await _repositorio.Consultar(u => u.IdUsuario == entidad.IdUsuario);

                Usuario usuario_editar = queryUsuario.First();
                usuario_editar.Nombre = entidad.Nombre;
                usuario_editar.Correo = entidad.Correo;
                usuario_editar.Telefono = entidad.Telefono;
                usuario_editar.IdRol = entidad.IdRol;
                usuario_editar.EsActivo = entidad.EsActivo;

                if (string.IsNullOrWhiteSpace(usuario_editar.NombreFoto))
                {
                    usuario_editar.NombreFoto = NombreFoto;
                }

                if (Foto != null)
                {
                    string urlFoto = await _firebaseService.SubirStorege(Foto, "carpeta_usuario", usuario_editar.NombreFoto ?? string.Empty);
                    usuario_editar.UrlFoto = urlFoto;
                }
                bool respuesta = await _repositorio.Editar(usuario_editar);

                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo modificar el usuario");
                }

                Usuario usuario_editado = queryUsuario.Include(r => r.IdRolNavigation).First();
                return usuario_editado;
            }
            catch
            {
                throw;
            }
        }

        /* Este método encapsula la lógica para eliminar un usuario del sistema, 
         * incluyendo su información de la base de datos y su imagen de perfil si existe. 
         * Devuelve true si la operación de eliminación se realiza correctamente. */
        public async Task<bool> Eliminar(int IdUsuario)
        {
            try
            {
                Usuario? usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == IdUsuario);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                string? nombreFoto = usuario_encontrado.NombreFoto;
                bool respuesta = await _repositorio.Eliminar(usuario_encontrado);

                if (respuesta && !string.IsNullOrWhiteSpace(nombreFoto))
                {
                    await _firebaseService.EliminarStorege("carpeta_usuario", nombreFoto);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        /* Este método busca un usuario en la base de datos utilizando las credenciales de correo electrónico y contraseña 
         * proporcionadas, y devuelve el usuario encontrado si existe, o null si no se encuentra ningún usuario. */
        public async Task<Usuario?> ObtenerPorCredenciales(string correo, string clave)
        {
            Usuario? usuario_encontrado = await _repositorio.Obtener(u => u.Correo.Equals(correo));

            if (usuario_encontrado == null)
            {
                return null;
            }

            bool credencialesValidas = _utilidadesService.VerificarClave(clave, usuario_encontrado.Clave);
            if (!credencialesValidas)
            {
                return null;
            }

            if (_utilidadesService.RequiereRehash(usuario_encontrado.Clave))
            {
                usuario_encontrado.Clave = _utilidadesService.HashearClave(clave);
                await _repositorio.Editar(usuario_encontrado);
            }

            return usuario_encontrado;
        }

        /* Este método encapsula la lógica para obtener un usuario de la base de datos por su ID, incluyendo la información de su rol asociado, y 
         * devuelve el usuario encontrado o null si no se encuentra ninguno. */
        public async Task<Usuario?> ObtenerPorId(int IdUsuario)
        {
            IQueryable<Usuario> query = await _repositorio.Consultar(u => u.IdUsuario == IdUsuario);

            Usuario? resultado = query.Include(r => r.IdRolNavigation).FirstOrDefault();
            return resultado;
        }

        /*Este método encapsula la lógica para guardar los cambios realizados en el perfil de un usuario en la base de datos y 
         * devuelve un valor booleano que indica si la operación fue exitosa. */
        public async Task<bool> GuardarPerfil(Usuario entidad)
        {
            try
            {
                Usuario? usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == entidad.IdUsuario);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                usuario_encontrado.Correo = entidad.Correo;
                usuario_encontrado.Telefono = entidad.Telefono;
                bool respuesta = await _repositorio.Editar(usuario_encontrado);
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        /* Este método encapsula la lógica para cambiar la contraseña de un usuario en la base de datos y 
         * devuelve un valor booleano que indica si la operación fue exitosa. */
        public async Task<bool> CambiarClave(int IdUsuario, string ClaveActual, string ClaveNueva)
        {
            try
            {
                Usuario? usuario_encontrado = await _repositorio.Obtener(u => u.IdUsuario == IdUsuario);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                if (!_utilidadesService.VerificarClave(ClaveActual, usuario_encontrado.Clave))
                {
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");
                }

                usuario_encontrado.Clave = _utilidadesService.HashearClave(ClaveNueva);
                bool respuesta = await _repositorio.Editar(usuario_encontrado);
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        /* Este método encapsula la lógica para restablecer la contraseña de un usuario, enviarle la nueva contraseña 
         * por correo electrónico y actualizar la contraseña en la base de datos, devolviendo 
         * un valor booleano que indica si la operación fue exitosa. */
        public async Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo)
        {
            try
            {
                Usuario? usuario_encontrado = await _repositorio.Obtener(u => u.Correo == Correo);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("No encontramos ningun usuario asociado al correo");
                }

                string clave_generada = _utilidadesService.GenerarClave();
                usuario_encontrado.Clave = _utilidadesService.HashearClave(clave_generada);

                UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[clave]", clave_generada);
                string htmlCorreo = await ObtenerHtmlPlantillaCorreoAsync(UrlPlantillaCorreo);

                bool correo_enviado = false;

                if (htmlCorreo != "")
                {
                    correo_enviado = await _correoService.EnviarCorreo(Correo, "Contraseña Restablecidad", htmlCorreo);
                }

                if (!correo_enviado)
                {
                    throw new TaskCanceledException("Tenemos problemas. Por favor inténtalo de nuevo más tarde");
                }
                bool respuesta = await _repositorio.Editar(usuario_encontrado);
                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}