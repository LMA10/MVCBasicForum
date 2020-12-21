using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo4.Data;
using BENT1C.Grupo4.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BENT1C.Grupo4.Controllers
{
    [Authorize]
    public class EntradasController : Controller
    {
        private readonly ForoDbContext _context;

        public EntradasController(ForoDbContext context)
        {
            _context = context;

        }
        //[Authorize(Roles = (nameof(Rol.Administrador)))]
        public async Task<IActionResult> Index()
        {
           
            var foroDbContext = _context.Entrada.Include(e => e.Categoria).Include(e => e.Miembro).Include(p => p.Preguntas).ThenInclude(r => r.Respuestas);
            return View(await foroDbContext.ToListAsync());
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .Include(e => e.Categoria)
                .Include(e => e.Miembro)
                .Include(p => p.Preguntas).ThenInclude(r => r.Respuestas)
                .ThenInclude(r => r.Reacciones)
                .Include(p => p.Preguntas).ThenInclude(r => r.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entrada == null)
            {
                return NotFound();
            }


            return View(entrada);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre");
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido");
            return View();
        }

        // POST: Entradas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Entrada entrada, string descripcion)
        {

            var miembroId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                entrada.Id = Guid.NewGuid();
                entrada.Fecha = DateTime.Now;
                entrada.MiembroId = miembroId;

                if (entrada.Privada)
                {
                    entrada.MiembrosHabilitados = new List<EntradaMiembro>();
                }

                entrada.Preguntas = new List<Pregunta>()
                {
                    new Pregunta
                    {
                        Id = Guid.NewGuid(),
                        Fecha = entrada.Fecha,
                        Descripcion = descripcion,
                        EntradaId = entrada.Id,
                        MiembroId = miembroId,
                        Activa = true
                    }
                };

                _context.Add(entrada);

                _context.SaveChanges();

                TempData["Nueva entrada"] = true;
                return RedirectToAction(nameof(Index), "Home");
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", entrada.MiembroId);

            return RedirectToAction("Home");
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", entrada.MiembroId);
            return View(entrada);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Titulo,Privada,CategoriaId,MiembroId")] Entrada entrada)
        {
            if (id != entrada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    entrada.Fecha = DateTime.Now;
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", entrada.MiembroId);
            return View(entrada);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .Include(e => e.Categoria)
                .Include(e => e.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entrada = await _context.Entrada.FindAsync(id);
            _context.Entrada.Remove(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        private bool EntradaExists(Guid id)
        {
            return _context.Entrada.Any(e => e.Id == id);
        }

        //public void QuantityLikesByUser()
        //{
        //    var miembroActualId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        //    var cant = _context.Like.Include(r => r.MeGusta)
        //        .Where(a => a == false)
        //        .Count(c => c.Entrada.MiembroId == miembroActualId);
        //    ViewData["cantidadLikes"] = cant;
        //}

    }
}
