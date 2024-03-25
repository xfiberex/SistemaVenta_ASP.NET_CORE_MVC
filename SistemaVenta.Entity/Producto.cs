namespace SistemaVenta.Entity;

public partial class Producto
{
    // El simbolo de interrogacion significa que permite valores nulos.
    // Todas estas propiedades o atributos deben estar igual que en la BD.

    public int IdProducto { get; set; }

    public string? CodigoBarra { get; set; }

    public string? Marca { get; set; }

    public string? Descripcion { get; set; }

    public int? IdCategoria { get; set; }

    public int? Stock { get; set; }

    public string? UrlImagen { get; set; }

    public string? NombreImagen { get; set; }

    public decimal? Precio { get; set; }

    public bool EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }
}
