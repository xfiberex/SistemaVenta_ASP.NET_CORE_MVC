using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Extensions;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;
using System.Security.Claims;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    // Si no esta autorizado por login no puede acceder
    [Authorize]
    public class VentaController : Controller
    {
        private readonly ITipoDocumentoVentaService _TipoDocumentoVentaService;
        private readonly IVentaService _ventaService;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private readonly ITimeLimitedDataProtector _pdfTokenProtector;

        public VentaController(ITipoDocumentoVentaService TipoDocumentoVentaService,
                               IVentaService ventaService,
                               IMapper mapper,
                               IConverter converter,
                               IDataProtectionProvider dataProtectionProvider)
        {
            _TipoDocumentoVentaService = TipoDocumentoVentaService;
            _ventaService = ventaService;
            _mapper = mapper;
            _converter = converter;
            _pdfTokenProtector = dataProtectionProvider
                .CreateProtector("SistemaVenta.PDFVenta.Token")
                .ToTimeLimitedDataProtector();
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }

        public IActionResult NuevaVenta()
        {
            return View();
        }

        private bool TryGetUserId(out int userId)
        {
            userId = 0;

            string? idUsuario = HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .SingleOrDefault();

            return int.TryParse(idUsuario, out userId);
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            List<VMTipoDocumentoVenta> vmListaTipoDocumentos = _mapper.Map<List<VMTipoDocumentoVenta>>(await _TipoDocumentoVentaService.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListaTipoDocumentos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMProducto> vmListaProductos = _mapper.Map<List<VMProducto>>(await _ventaService.ObtenerProductos(busqueda));
            return StatusCode(StatusCodes.Status200OK, vmListaProductos);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta> gResponse = new GenericResponse<VMVenta>();

            try
            {
                if (!TryGetUserId(out int idUsuario))
                {
                    gResponse.Estado = false;
                    gResponse.Mensaje = "Sesión inválida.";
                    return StatusCode(StatusCodes.Status401Unauthorized, gResponse);
                }

                modelo.IdUsuario = idUsuario;

                Venta venta_creada = await _ventaService.Registrar(_mapper.Map<Venta>(modelo));
                modelo = _mapper.Map<VMVenta>(venta_creada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Historial(string numeroVenta, string fechaInicio, string fechaFin)
        {
            List<VMVenta> vmHistorialVenta = _mapper.Map<List<VMVenta>>(await _ventaService.Historial(numeroVenta, fechaInicio, fechaFin));
            return StatusCode(StatusCodes.Status200OK, vmHistorialVenta);
        }


        // Para la devolucion del PDF generado
        public IActionResult MostrarPDFVenta(string numeroVenta)
        {
            string token = _pdfTokenProtector.Protect(numeroVenta, TimeSpan.FromMinutes(5));
            string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}&token={Uri.EscapeDataString(token)}";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page = urlPlantillaVista
                    }
                }
            };

            var archivoPDF = _converter.Convert(pdf);

            return File(archivoPDF, "application/pdf");
        }
    }
}