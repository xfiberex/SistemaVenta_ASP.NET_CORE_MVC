using SistemaVenta.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SistemaVenta.BLL.Implementacion
{
    public class UtilidadesService : IUtilidadesService
    {
        /* Este método genera y devuelve una clave aleatoria de 6 caracteres que puede ser 
         * utilizada, por ejemplo, para autenticación, generación de contraseñas temporales, o cualquier 
         * otro propósito que requiera una cadena aleatoria. */
        public string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }

        /* Este método toma una cadena de texto, calcula su hash SHA-256 y devuelve la representación hexadecimal del hash. */
        public string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
