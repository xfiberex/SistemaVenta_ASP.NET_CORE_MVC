namespace SistemaVenta.Entity;

public partial class DetalleVenta
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public string MarcaProducto { get; set; } = null!;

    public string DescripcionProducto { get; set; } = null!;

    public string CategoriaProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public decimal Total { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
