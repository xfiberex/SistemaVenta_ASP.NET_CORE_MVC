namespace SistemaVenta.AplicacionWeb.Models.ViewModels
{
    public class VMMenu
    {
        public string? Descripcion { get; set; }

        public string? Icono { get; set; }

        public string? Controlador { get; set; }

        public string? PaginaAccion { get; set; }

        public virtual ICollection<VMMenu> SubMenus { get; set; } = new List<VMMenu>();
    }
}
