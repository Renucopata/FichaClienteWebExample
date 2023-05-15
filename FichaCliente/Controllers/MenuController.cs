using FichaCliente.Models;
using FichaCliente.ServicioWeb;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FichaCliente.Controllers
{
    public class MenuController : Controller
    {
        private readonly InterfazSaul servicio;

        public MenuController(InterfazSaul serv)
        {
            servicio = serv;
        }

        public IActionResult Sesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sesion(ModeloUsuario usuario, string us, string pass)
        {
            usuario = servicio.Autenticacion(us, pass);
            if (usuario != null)
            {
                if (usuario.login == "SIN RESPUESTA")
                {
                    return RedirectToAction("ERRORsaul", "Menu");
                }
                else
                {
                    Int64 rol = usuario.codigo_agenda;
                    bool resp = servicio.Cargos(rol);
                    if (resp)
                    {
                        var coockie = new List<Claim>
                     {
                         new Claim(ClaimTypes.Name,usuario.login),
                     };
                        coockie.Add(new Claim(ClaimTypes.Role, usuario.cargo));
                        var identidad = new ClaimsIdentity(coockie, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));
                        return RedirectToAction("Paneles", "Fichas");
                    }
                    else
                    {
                        return RedirectToAction("Restriccion", "Menu");
                    }

                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Restriccion()
        {
            return View();
        }

        public IActionResult ERRORsaul()
        {
            return View();
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Sesion", "Menu");
        }
    }
}
