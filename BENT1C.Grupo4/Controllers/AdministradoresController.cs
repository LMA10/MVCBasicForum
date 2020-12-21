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
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace BENT1C.Grupo4.Controllers
{
    [Authorize(Roles = nameof(Rol.Administrador))]
    public class AdministradoresController : Controller
    {
        private readonly ForoDbContext _context;

        public AdministradoresController(ForoDbContext context)
        {
            _context = context;
        }

        // GET: Administradores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administrador.ToListAsync());
        }

        // GET: Administradores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administrador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // GET: Administradores/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = nameof(Rol.Administrador))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Administrador administrador, string pass)
        {

            try
            {
                pass.ValidarPassword();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(nameof(Administrador.Password), ex.Message);
            }




            if (ModelState.IsValid)
            {
                administrador.Password = pass.Encriptar();
                administrador.FechaAlta = DateTime.Now;
                administrador.Id = Guid.NewGuid();
                _context.Add(administrador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(administrador);
        }

        // GET: Administradores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administrador.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return View(administrador);
        }

        // POST: Administradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nombre,Apellido,Email")] Administrador administrador)
        {
            if (id != administrador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    administrador.FechaAlta = DateTime.Now;
                    _context.Update(administrador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministradorExists(administrador.Id))
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
            return View(administrador);
        }

        // GET: Administradores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administrador
                .FirstOrDefaultAsync(m => m.Id == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // POST: Administradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var administrador = await _context.Administrador.FindAsync(id);
            _context.Administrador.Remove(administrador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministradorExists(Guid id)
        {
            return _context.Administrador.Any(e => e.Id == id);
        }
    }
}
