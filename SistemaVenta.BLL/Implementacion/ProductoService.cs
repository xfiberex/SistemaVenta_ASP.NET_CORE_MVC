using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _repositorio;
        private readonly IFireBaseService _firebaseService;
        private readonly IUsuarioService _usuarioService;

        public ProductoService(IGenericRepository<Producto> repositorio, IFireBaseService firebaseService, IUsuarioService usuarioService)
        {
            _repositorio = repositorio;
            _firebaseService = firebaseService;
            _usuarioService = usuarioService;
        }

        public async Task<List<Producto>> Lista()
        {
            IQueryable<Producto> query = await _repositorio.Consultar();
            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }

        public async Task<Producto> Crear(Producto entidad, Stream imagenStream = null, string nombreImagen = "")
        {
            Producto producto_existe = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra);

            if (producto_existe != null)
                throw new Exception("El código de barras ya existe");

            try
            {
                entidad.NombreImagen = nombreImagen;

                if (imagenStream != null)
                {
                    string urlImagen = await _firebaseService.SubirStorege(imagenStream, "carpeta_producto", nombreImagen);
                    entidad.UrlImagen = urlImagen;
                }

                Producto producto_creado = await _repositorio.Crear(entidad);

                if (producto_creado == null)
                    throw new Exception("No se pudo crear el producto");

                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == producto_creado.IdProducto);
                producto_creado = query.Include(c => c.IdCategoriaNavigation).FirstOrDefault();

                return producto_creado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el producto: " + ex.Message);
            }
        }

        public async Task<Producto> Editar(Producto entidad, Stream imagen = null, string NombreImagen = "")
        {
            Producto producto_existe = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra && p.IdProducto != entidad.IdProducto);

            if (producto_existe != null)
                throw new TaskCanceledException("El codigo de barra ya existe");

            try
            {
                IQueryable<Producto> queryProducto = await _repositorio.Consultar(p => p.IdProducto == entidad.IdProducto);

                Producto producto_editar = queryProducto.First();

                producto_editar.CodigoBarra = entidad.CodigoBarra;
                producto_editar.Marca = entidad.Marca;
                producto_editar.Descripcion = entidad.Descripcion;
                producto_editar.IdCategoria = entidad.IdCategoria;
                producto_editar.Stock = entidad.Stock;
                producto_editar.Precio = entidad.Precio;
                producto_editar.EsActivo = entidad.EsActivo;

                if (producto_editar.NombreImagen == "")
                {
                    producto_editar.NombreImagen = NombreImagen;
                }

                if (imagen != null)
                {
                    string urlImagen = await _firebaseService.SubirStorege(imagen, "carpeta_producto", producto_editar.NombreImagen);
                    producto_editar.UrlImagen = urlImagen;
                }

                bool respuesta = await _repositorio.Editar(producto_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo editar el producto");

                Producto producto_editado = queryProducto.Include(c => c.IdCategoriaNavigation).First();

                return producto_editado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                Producto producto_encontrado = await _repositorio.Obtener(p => p.IdProducto == idProducto);

                if (producto_encontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                string nombreImagen = producto_encontrado.NombreImagen;

                bool respuesta = await _repositorio.Eliminar(producto_encontrado);

                if (respuesta)
                    await _firebaseService.EliminarStorege("carpeta_producto", nombreImagen);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
