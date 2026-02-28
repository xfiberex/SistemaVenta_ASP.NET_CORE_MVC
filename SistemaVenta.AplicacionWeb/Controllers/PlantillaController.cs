using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Extensions;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.BLL.Interfaces;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class PlantillaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INegocioService _negocioService;
        private readonly IVentaService _ventaService;
        private readonly ITimeLimitedDataProtector _pdfTokenProtector;

        public PlantillaController(IMapper mapper, INegocioService negocioService, IVentaService ventaService, IDataProtectionProvider dataProtectionProvider)
        {
            _mapper = mapper;
            _negocioService = negocioService;
            _ventaService = ventaService;
            _pdfTokenProtector = dataProtectionProvider
                .CreateProtector("SistemaVenta.PDFVenta.Token")
                .ToTimeLimitedDataProtector();
        }

        /* Este método prepara y envía los datos necesarios a una vista 
         * para que pueda mostrar información como el correo electrónico, 
         * la clave y la URL de manera dinámica. */
        [AllowAnonymous]
        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"] = correo;
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> PDFVenta(string numeroVenta, string token)
        {
            if (string.IsNullOrWhiteSpace(numeroVenta) || string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized();
            }

            try
            {
                string payload = _pdfTokenProtector.Unprotect(token);
                if (!string.Equals(payload, numeroVenta, StringComparison.Ordinal))
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return Unauthorized();
            }

            VMVenta vmVenta = _mapper.Map<VMVenta>(await _ventaService.Detalle(numeroVenta));
            VMNegocio vmNegocio = _mapper.Map<VMNegocio>(await _negocioService.Obtener());
            VMPDFVenta modelo = new VMPDFVenta();

            modelo.negocio = vmNegocio;
            modelo.venta = vmVenta;

            return View(modelo);
        }

        // Similar a la anterior, pero solo prepara y envia la clave
        [AllowAnonymous]
        public IActionResult RestablecerClave(string clave)
        {
            ViewData["Clave"] = clave;
            return View();
        }
    }
}
