using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> ObtenerMenus(int idUsuario);
    }
}
