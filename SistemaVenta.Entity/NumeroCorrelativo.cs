namespace SistemaVenta.Entity;

public partial class NumeroCorrelativo
{
    public int IdNumeroCorrelativo { get; set; }

    public int UltimoNumero { get; set; }

    public int CantidadDigitos { get; set; }

    public string Gestion { get; set; } = null!;

    public DateTime FechaActualizacion { get; set; }
}
