using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo4.Data;
using BENT1C.Grupo4.Models;
using BENT1C.Grupo4.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BENT1C.Grupo4.Controllers
{
    [AllowAnonymous]
    public class MiembrosController : Controller
    {
        private readonly ForoDbContext _context;

        public MiembrosController(ForoDbContext context)
        {
            _context = context;
        }

        // GET: Miembros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Miembro.ToListAsync());
        }

        // GET: Miembros/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // GET: Miembros/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Miembros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Telefono,Id,Nombre,Apellido,Email")] Miembro miembro, string pass)
        {

            try
            {
                pass.ValidarPassword();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Administrador.Password), ex.Message);
            }


            if (ModelState.IsValid)
            {
                miembro.Password = pass.Encriptar();
                miembro.FechaAlta = DateTime.Now;
                miembro.Id = Guid.NewGuid();
                _context.Add(miembro);
                await _context.SaveChangesAsync();

                //Autenticar(miembro.Email,pass);

                return RedirectToAction("Index","Home");
            }
            return View(miembro);
        }

        //public IActionResult Autenticar(string username, string password)
        //{

        //    Usuario usuario = _context.Miembro.FirstOrDefault(miembro => miembro.Email == username);

        //    var passwordEncriptada = password.Encriptar();

        //    if (usuario.Password.SequenceEqual(passwordEncriptada))
        //    {
        //        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        //        identity.AddClaim(new Claim(ClaimTypes.Name, username));

        //        identity.AddClaim(new Claim(ClaimTypes.Role, usuario.Rol.ToString()));

        //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

        //        identity.AddClaim(new Claim(ClaimTypes.GivenName, usuario.Nombre));

        //        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //        //Aca sucede la autenticacion (Se crea la cookie)
        //        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //    }
        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}

        // GET: Miembros/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembro.FindAsync(id);
            if (miembro == null)
            {
                return NotFound();
            }
            return View(miembro);
        }

        // POST: Miembros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Telefono,Id,Nombre,Apellido,Email")] Miembro miembro)
        {
            if (id != miembro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    miembro.FechaAlta = DateTime.Now;
                    _context.Update(miembro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        // GET: Miembros/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var miembro = await _context.Miembro.FindAsync(id);
            _context.Miembro.Remove(miembro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembroExists(Guid id)
        {
            return _context.Miembro.Any(e => e.Id == id);
        }
    }
}
