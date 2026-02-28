using SistemaVenta.BLL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SistemaVenta.BLL.Implementacion
{
    public class UtilidadesService : IUtilidadesService
    {
        private const int Pbkdf2Iteraciones = 100000;
        private const int Pbkdf2TamanoSalt = 16;
        private const int Pbkdf2TamanoHash = 32;

        /* Este método genera y devuelve una clave aleatoria de 6 caracteres que puede ser 
         * utilizada, por ejemplo, para autenticación, generación de contraseñas temporales, o cualquier 
         * otro propósito que requiera una cadena aleatoria. */
        public string GenerarClave()
        {
            const string alfabeto = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            const int longitud = 12;

            byte[] randomBytes = RandomNumberGenerator.GetBytes(longitud);
            StringBuilder clave = new StringBuilder(longitud);

            foreach (byte randomByte in randomBytes)
            {
                clave.Append(alfabeto[randomByte % alfabeto.Length]);
            }

            return clave.ToString();
        }

        /* Este método toma una cadena de texto, calcula su hash SHA-256 y devuelve la representación hexadecimal del hash. */
        public string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
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

        public string HashearClave(string clave)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(Pbkdf2TamanoSalt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                clave,
                salt,
                Pbkdf2Iteraciones,
                HashAlgorithmName.SHA256,
                Pbkdf2TamanoHash);

            return $"PBKDF2${Pbkdf2Iteraciones}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public bool VerificarClave(string clavePlano, string hashGuardado)
        {
            if (string.IsNullOrWhiteSpace(hashGuardado))
            {
                return false;
            }

            if (!hashGuardado.StartsWith("PBKDF2$", StringComparison.Ordinal))
            {
                return string.Equals(ConvertirSha256(clavePlano), hashGuardado, StringComparison.OrdinalIgnoreCase);
            }

            string[] partes = hashGuardado.Split('$');
            if (partes.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(partes[1], out int iteraciones) || iteraciones <= 0)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(partes[2]);
            byte[] hashEsperado = Convert.FromBase64String(partes[3]);

            byte[] hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                clavePlano,
                salt,
                iteraciones,
                HashAlgorithmName.SHA256,
                hashEsperado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
        }

        public bool RequiereRehash(string hashGuardado)
        {
            return string.IsNullOrWhiteSpace(hashGuardado) || !hashGuardado.StartsWith("PBKDF2$", StringComparison.Ordinal);
        }
    }
}
