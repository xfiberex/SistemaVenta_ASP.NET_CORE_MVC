namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMUsuarioLogin
    {
        public string? Correo { get; set; }
        public string? Clave { get; set; }
        public bool MantenerSesion { get; set; }
    }
}
