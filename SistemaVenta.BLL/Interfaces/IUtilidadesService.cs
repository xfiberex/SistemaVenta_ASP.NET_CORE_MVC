namespace SistemaVenta.BLL.Interfaces
{
    public interface IUtilidadesService
    {
        string GenerarClave();
        string ConvertirSha256(string texto);
        string HashearClave(string clave);
        bool VerificarClave(string clavePlano, string hashGuardado);
        bool RequiereRehash(string hashGuardado);
    }
}
