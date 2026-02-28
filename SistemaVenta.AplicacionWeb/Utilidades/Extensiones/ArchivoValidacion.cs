namespace SistemaVenta.AplicacionWeb.Utilidades.Extensiones
{
    public static class ArchivoValidacion
    {
        private static readonly HashSet<string> ExtensionesPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        public const long MaximoTamanoImagenBytes = 2 * 1024 * 1024;

        public static bool EsImagenValida(IFormFile? archivo, out string mensaje)
        {
            mensaje = string.Empty;

            if (archivo == null)
            {
                return true;
            }

            if (archivo.Length <= 0)
            {
                mensaje = "El archivo seleccionado está vacío.";
                return false;
            }

            if (archivo.Length > MaximoTamanoImagenBytes)
            {
                mensaje = "El archivo excede el tamaño máximo permitido de 2 MB.";
                return false;
            }

            string extension = Path.GetExtension(archivo.FileName);
            if (string.IsNullOrWhiteSpace(extension) || !ExtensionesPermitidas.Contains(extension))
            {
                mensaje = "Formato de imagen no permitido. Use jpg, jpeg, png o webp.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(archivo.ContentType) || !archivo.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                mensaje = "El contenido del archivo no corresponde a una imagen válida.";
                return false;
            }

            return true;
        }
    }
}
