using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.Interfaces;
using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Implementacion
{
    public class NegocioService : INegocioService
    {
        private readonly IGenericRepository<Negocio> _repositorio;
        private readonly IFireBaseService _firebaseService;

        public NegocioService(IGenericRepository<Negocio> repositorio, IFireBaseService firebase)
        {
            _repositorio = repositorio;
            _firebaseService = firebase;
        }

        public async Task<Negocio> Obtener()
        {
            try
            {
                Negocio? negocio_encontrado = await _repositorio.Obtener(n => n.IdNegocio == 1);
                if (negocio_encontrado == null)
                {
                    throw new TaskCanceledException("No se encontró la configuración del negocio");
                }

                return negocio_encontrado;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Negocio> GuardarCambios(Negocio entidad, Stream? Logo = null, string NombreLogo = "")
        {
            try
            {
                Negocio? negocio_encontrado = await _repositorio.Obtener(n => n.IdNegocio == 1);
                if (negocio_encontrado == null)
                {
                    throw new TaskCanceledException("No se encontró la configuración del negocio");
                }

                negocio_encontrado.NumeroDocumento = entidad.NumeroDocumento;
                negocio_encontrado.Nombre = entidad.Nombre;
                negocio_encontrado.Correo = entidad.Correo;
                negocio_encontrado.Direccion = entidad.Direccion;
                negocio_encontrado.Telefono = entidad.Telefono;
                negocio_encontrado.PorcentajeImpuesto = entidad.PorcentajeImpuesto;
                negocio_encontrado.SimboloMoneda = entidad.SimboloMoneda;
                negocio_encontrado.NombreLogo = string.IsNullOrWhiteSpace(negocio_encontrado.NombreLogo) ? NombreLogo : negocio_encontrado.NombreLogo;

                if (Logo != null)
                {
                    string urlLogo = await _firebaseService.SubirStorege(Logo, "carpeta_logo", negocio_encontrado.NombreLogo ?? string.Empty);
                    negocio_encontrado.UrlLogo = urlLogo;
                }

                await _repositorio.Editar(negocio_encontrado);
                return negocio_encontrado;

            }
            catch
            {
                throw;
            }
        }
    }
}
