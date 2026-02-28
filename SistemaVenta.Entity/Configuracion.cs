namespace SistemaVenta.Entity;

public partial class Configuracion
{
    public string Recurso { get; set; } = null!;

    public string Propiedad { get; set; } = null!;

    public string? Valor { get; set; }
}
