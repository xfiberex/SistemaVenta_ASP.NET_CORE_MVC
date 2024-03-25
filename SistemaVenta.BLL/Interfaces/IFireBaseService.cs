namespace SistemaVenta.BLL.Interfaces
{
    public interface IFireBaseService
    {
        Task<string> SubirStorege(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo);
        Task<bool> EliminarStorege(string CarpetaDestino, string NombreArchivo);
    }
}
