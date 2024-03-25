using SistemaVenta.Entity;

namespace SistemaVenta.DAL.Interfaces
{
    // Interface o estructura de venta
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta entidad);

        Task<List<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin);
    }
}
