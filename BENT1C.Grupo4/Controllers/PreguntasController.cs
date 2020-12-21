using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BENT1C.Grupo4.Data;
using BENT1C.Grupo4.Models;
using Microsoft.AspNetCore.Authorization;

namespace BENT1C.Grupo4.Controllers
{
    [Authorize]
    public class PreguntasController : Controller
    {
        private readonly ForoDbContext _context;

        public PreguntasController(ForoDbContext context)
        {
            _context = context;
        }

        // GET: Preguntas
        public async Task<IActionResult> Index()
        {
            var foroDbContext = _context.Pregunta.Include(p => p.Entrada).Include(p => p.Miembro);
            return View(await foroDbContext.ToListAsync());
        }

        // GET: Preguntas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Pregunta
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // GET: Preguntas/Create
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_context.Entrada, "Id", "Titulo");
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido");
            return View();
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string textArea, Guid idEntrada, Guid miembroId)
        {
            if (!User.IsInRole(nameof(Rol.Administrador)))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(new Pregunta
                    {
                        Id = Guid.NewGuid(),
                        Fecha = DateTime.Now,
                        Activa = true,
                        Descripcion = textArea,
                        EntradaId = idEntrada,
                        MiembroId = miembroId
                    });

                    _context.SaveChanges();

                    return RedirectToAction("Details", "Entradas", new { id = idEntrada });
                }

            }
            else
            {
                TempData["validaPreguntaAdmin"] = true;
            }

            return RedirectToAction("Details","Entradas", new { id = idEntrada });
        }

        // GET: Preguntas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Pregunta.FindAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_context.Entrada, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // POST: Preguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Descripcion,Activa,EntradaId,MiembroId")] Pregunta pregunta)
        {
            if (id != pregunta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pregunta.Fecha = DateTime.Now;
                    _context.Update(pregunta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreguntaExists(pregunta.Id))
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
            ViewData["EntradaId"] = new SelectList(_context.Entrada, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // GET: Preguntas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _context.Pregunta
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pregunta = await _context.Pregunta.FindAsync(id);
            _context.Pregunta.Remove(pregunta);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        private bool PreguntaExists(Guid id)
        {
            return _context.Pregunta.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult CambiarEstadoPregunta(Guid preguntaId, Guid entradaId)
        {
            Pregunta p = new Pregunta();

            if (PreguntaExists(preguntaId))
            {
                p = _context.Pregunta.Find(preguntaId);
            }

            if (p.Activa)
            {
                p.Activa = false;
            }
            else
            {
                p.Activa = true;
            }

            _context.Update(p);
            _context.SaveChanges();

            return RedirectToAction("Details", "Entradas", new { id = entradaId });

        }
    }
}
