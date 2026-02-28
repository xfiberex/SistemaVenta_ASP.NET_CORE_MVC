using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.AplicacionWeb.Models;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;
using System.Diagnostics;
using System.Security.Claims;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    // Si no esta autorizado por login no puede acceder
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,
                              IUsuarioService usuarioService,
                              IMapper mapper)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Perfil()
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
        public async Task<IActionResult> ObtenerUsuario()
        {
            GenericResponse<VMUsuario> response = new GenericResponse<VMUsuario>();

            try
            {
                if (!TryGetUserId(out int idUsuario))
                {
                    response.Estado = false;
                    response.Mensaje = "Sesión inválida.";
                    return StatusCode(StatusCodes.Status401Unauthorized, response);
                }

                Usuario? usuarioEntidad = await _usuarioService.ObtenerPorId(idUsuario);
                if (usuarioEntidad == null)
                {
                    response.Estado = false;
                    response.Mensaje = "Usuario no encontrado.";
                    return StatusCode(StatusCodes.Status404NotFound, response);
                }

                VMUsuario usuario = _mapper.Map<VMUsuario>(usuarioEntidad);

                response.Estado = true;
                response.Objeto = usuario;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPerfil([FromBody] VMUsuario modelo)
        {
            GenericResponse<VMUsuario> response = new GenericResponse<VMUsuario>();

            try
            {
                if (!TryGetUserId(out int idUsuario))
                {
                    response.Estado = false;
                    response.Mensaje = "Sesión inválida.";
                    return StatusCode(StatusCodes.Status401Unauthorized, response);
                }

                Usuario entidad = _mapper.Map<Usuario>(modelo);
                entidad.IdUsuario = idUsuario;

                bool resultado = await _usuarioService.GuardarPerfil(entidad);

                response.Estado = resultado;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarClave([FromBody] VMCambiarClave modelo)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();

            try
            {
                if (!TryGetUserId(out int idUsuario))
                {
                    response.Estado = false;
                    response.Mensaje = "Sesión inválida.";
                    return StatusCode(StatusCodes.Status401Unauthorized, response);
                }

                if (string.IsNullOrWhiteSpace(modelo.claveActual) || string.IsNullOrWhiteSpace(modelo.claveNueva))
                {
                    response.Estado = false;
                    response.Mensaje = "Debe ingresar la clave actual y la nueva clave.";
                    return StatusCode(StatusCodes.Status400BadRequest, response);
                }

                // Identificar al usuario para cambiar su clave
                bool resultado = await _usuarioService.CambiarClave(
                    idUsuario,
                    modelo.claveActual,
                    modelo.claveNueva);

                response.Estado = resultado;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Acceso");
        }
    }
}
