namespace SistemaVenta.Entity;

public partial class Usuario
{
    public Usuario()
    {
        Venta = new HashSet<Venta>();
    }

    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public int IdRol { get; set; }

    public string? UrlFoto { get; set; }

    public string? NombreFoto { get; set; }

    public string Clave { get; set; } = null!;

    public bool EsActivo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; }
}
