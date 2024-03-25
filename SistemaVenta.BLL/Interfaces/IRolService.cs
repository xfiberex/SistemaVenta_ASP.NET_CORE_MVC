using SistemaVenta.Entity;

namespace SistemaVenta.BLL.Interfaces
{
    public interface IRolService
    {
        Task<List<Rol>> Lista();
    }
}
