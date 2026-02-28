namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMDashBoard
    {
        public int TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public int TotalCategorias { get; set; }
        public List<VMVentasSemana> VentasUltimaSemana { get; set; } = new();
        public List<VMProductosSemana> ProductosTopUltimaSemana { get; set; } = new();
    }
}
