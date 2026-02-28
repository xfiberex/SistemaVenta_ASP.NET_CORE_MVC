using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SistemaVenta.AplicacionWeb.Utilidades.ViewComponents
{
    public class MenuUsuarioViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            string nombreUsuario = "";
            string urlFotoUsuario = "";

            if (claimUser.Identity?.IsAuthenticated == true)
            {
                nombreUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault() ?? string.Empty;

                ClaimsIdentity? identity = claimUser.Identity as ClaimsIdentity;
                urlFotoUsuario = identity?.FindFirst("UrlFoto")?.Value ?? string.Empty;
            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["urlFotoUsuario"] = urlFotoUsuario;

            return View();
        }
    }
}
