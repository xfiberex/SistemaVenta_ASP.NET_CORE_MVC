using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;
using System.Security.Claims;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AccesoController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "DashBoard");
            }
            return View();
        }

        public IActionResult RestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            try
            {
                Usuario usuario_encontrado = await _usuarioService.ObtenerPorCredenciales(modelo.Correo, modelo.Clave);

                if (usuario_encontrado == null)
                {
                    ViewData["Mensaje"] = "No se encontraron coincidencias. El correo o la constraseña, son incorrectos.";
                    return View();
                }

                ViewData["Mensaje"] = null;

                List<Claim> claims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario_encontrado.IdRol.ToString()),
                new Claim("UrlFoto", usuario_encontrado.UrlFoto),
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelo.MantenerSesion,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "DashBoard");
            }
            catch (Exception)
            {
                ViewData["Mensaje"] = "Por favor, asegúrese de haber ingresado su correo y contraseña.";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerClave(VMUsuarioLogin modelo)
        {
            try
            {
                if (string.IsNullOrEmpty(modelo.Correo))
                {
                    ViewData["MensajeCampoVacio"] = "Por favor, asegúrese de haber ingresado el correo";
                    return View(); // Devuelve la vista sin continuar
                }

                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/RestablecerClave?clave=[clave]";

                bool resultado = await _usuarioService.RestablecerClave(modelo.Correo, urlPlantillaCorreo);

                if (resultado)
                {
                    ViewData["Mensaje"] = "Listo, su contraseña fue restablecida. Revise su correo";
                    ViewData["MensajeError"] = null;
                }
                else
                {
                    ViewData["MensajeError"] = "Tenemos problemas. Por favor inténtelo de nuevo más tarde";
                    ViewData["Mensaje"] = null;
                }
            }
            catch (Exception ex)
            {
                ViewData["MensajeError"] = "Tenemos problemas. Por favor inténtelo de nuevo más tarde: " + ex.Message;
                ViewData["Mensaje"] = null;
            }
            return View();
        }
    }
}
