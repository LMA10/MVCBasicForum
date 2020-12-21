using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BENT1C.Grupo4.Data;
using BENT1C.Grupo4.Extensions;
using BENT1C.Grupo4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BENT1C.Grupo4.Controllers
{
    [AllowAnonymous]
    public class AccesosController : Controller
    {

        private readonly ForoDbContext _context;
        private const string _Return_Url = "ReturnUrl";

        public AccesosController(ForoDbContext context) 
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult Ingresar(string returnUrl) 
        {
            TempData[_Return_Url] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Ingresar(string username, string password)
        {
            string returnUrl = TempData[_Return_Url] as string;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {

                //Busqueda del miembro que coincida con el username que entra por parametro
                Usuario usuario = _context.Miembro.FirstOrDefault(miembro => miembro.Email == username);

                if (usuario == null)
                {
                    //Busqueda del adminsitrador que coincida con el username que entra por parametro
                    usuario = _context.Administrador.FirstOrDefault(admin => admin.Email == username);
                }

                if (usuario != null)
                {
                    var passwordEncriptada = password.Encriptar();

                    //Busqueda dentro de usuario y se fija si coincide con la password encriptada
                    if (usuario.Password.SequenceEqual(passwordEncriptada))
                    {
                        //Se crea el objeto Identity donde guardaremos las propieades del usuario a utilizar  (Cookie)
                        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                        identity.AddClaim(new Claim(ClaimTypes.Name, username));

                        identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));

                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

                        identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));

                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        //Aca sucede la autenticacion (Se crea la cookie)
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        TempData["Logged in"] = true;

                        if (!string.IsNullOrWhiteSpace(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            ViewBag.UserName = username;
            TempData[_Return_Url] = returnUrl;


            return View();
        }

       

        [Authorize]
        [HttpGet]
        public IActionResult NoAutorizado()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Salir()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Log out"] = true;

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
