namespace SistemaVenta.BLL.Interfaces
{
    public interface IDashBoardService
    {
        Task<int> TotalVentasUltimaSemana();
        Task<string> TotalIngresosUltimaSemana();
        Task<int> TotalProductos();
        Task<int> TotalCategorias();
        Task<Dictionary<string, int>> VentasUltimaSemana();
        Task<Dictionary<string, int>> ProductosTopUltimaSemana();
    }
}
