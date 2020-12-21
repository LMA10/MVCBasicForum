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
    public class RespuestasController : Controller
    {
        private readonly ForoDbContext _context;

        public RespuestasController(ForoDbContext context)
        {
            _context = context;
        }

        // GET: Respuestas
        public async Task<IActionResult> Index()
        {
            var foroDbContext = _context.Respuesta.Include(r => r.Miembro).Include(r => r.Pregunta);
            return View(await foroDbContext.ToListAsync());
        }

        // GET: Respuestas/Details/5
        public  IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta =  _context.Respuesta
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .Where(m => m.PreguntaId == id).ToList();
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // GET: Respuestas/Create
        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido");
            ViewData["PreguntaId"] = new SelectList(_context.Pregunta, "Id", "Descripcion");
            return View();
        }

        // POST: Respuestas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid miembroId,Guid preguntaId, string respuestaDesc, Guid entradaId)
        {
            if (!User.IsInRole(nameof(Rol.Administrador)))
            {
                Respuesta respuesta = new Respuesta();

                if (ModelState.IsValid)
                {
                    respuesta.Fecha = DateTime.Now;
                    respuesta.Id = Guid.NewGuid();
                    respuesta.Descripcion = respuestaDesc;
                    respuesta.MiembroId = miembroId;
                    respuesta.PreguntaId = preguntaId;
                    _context.Add(respuesta);
                    _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Entradas", new { id = entradaId });
                }
            }
            else 
            {
                TempData["validaPreguntaAdmin"] = true;
            }

            //ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", respuesta.MiembroId);
            //ViewData["PreguntaId"] = new SelectList(_context.Pregunta, "Id", "Descripcion", respuesta.PreguntaId);
            return RedirectToAction("Details", "Entradas", new { id = entradaId });
        }

        // GET: Respuestas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuesta.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Pregunta, "Id", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // POST: Respuestas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Descripcion,PreguntaId,MiembroId")] Respuesta respuesta)
        {
            if (id != respuesta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    respuesta.Fecha = DateTime.Now;
                    _context.Update(respuesta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespuestaExists(respuesta.Id))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Pregunta, "Id", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuesta
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var respuesta = await _context.Respuesta.FindAsync(id);
            _context.Respuesta.Remove(respuesta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespuestaExists(Guid id)
        {
            return _context.Respuesta.Any(e => e.Id == id);
        }
    }
}
