namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMVenta
    {
        public VMVenta()
        {
            DetalleVenta = new HashSet<VMDetalleVenta>();
        }

        public int IdVenta { get; set; }

        public string? NumeroVenta { get; set; }

        public int? IdTipoDocumentoVenta { get; set; }

        public string? TipoDocumentoVenta { get; set; }

        public int? IdUsuario { get; set; }

        public string? Usuario { get; set; }

        public string? DocumentoCliente { get; set; }

        public string? NombreCliente { get; set; }

        public string? SubTotal { get; set; }

        public string? ImpuestoTotal { get; set; }

        public string? Total { get; set; }

        public string? FechaRegistro { get; set; }

        public virtual ICollection<VMDetalleVenta> DetalleVenta { get; set; }
    }
}
