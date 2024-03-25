using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface INegocioService
    {
        Task<Negocio> Obtener();
        Task<Negocio> GuardarCambios(Negocio entidad, Stream Logo = null, string NombreLogo = "");
    }
}
