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
using System.Security.Claims;

namespace BENT1C.Grupo4.Controllers
{
    [Authorize]
    public class EntradaMiembrosController : Controller
    {
        private readonly ForoDbContext _context;

        public EntradaMiembrosController(ForoDbContext context)
        {
            _context = context;
        }

        // GET: EntradaMiembros
        //public async Task<IActionResult> Index()
        //{
        //    var foroDbContext = _context.EntradaMiembro.Include(e => e.Entrada).Include(e => e.Miembro);
        //    return View(await foroDbContext.ToListAsync());
        //}

        public IActionResult Index(Guid? id)
        {

            var foroDbContext = _context.EntradaMiembro
                .Include(e => e.Entrada)
                .Include(e => e.Miembro)
                .Where(em => em.Entrada.Miembro == _context.Miembro.Find(id)).ToList();

            return View(foroDbContext.ToList());
        }

        // GET: EntradaMiembros/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entradaMiembro = await _context.EntradaMiembro
                .Include(e => e.Entrada)
                .Include(e => e.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entradaMiembro == null)
            {
                return NotFound();
            }

            return View(entradaMiembro);
        }

        // GET: EntradaMiembros/Create
        public IActionResult Create()
        {
            ViewData["IdEntrada"] = new SelectList(_context.Entrada, "Id", "Titulo");
            ViewData["IdMiembro"] = new SelectList(_context.Miembro, "Id", "Apellido");
            return View();
        }

        // POST: EntradaMiembros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntradaMiembro entradaMiembro)
        {

            
            var eMiembroExist = _context
                .EntradaMiembro.Where(e => e.IdEntrada == entradaMiembro.IdEntrada && e.IdMiembro == entradaMiembro.IdMiembro)
                .FirstOrDefault();

            if (eMiembroExist == null)
            {
                if (ModelState.IsValid)
                {
                    entradaMiembro.Id = Guid.NewGuid();
                    _context.Add(entradaMiembro);
                    await _context.SaveChangesAsync();
                    TempData["peticionEntradaRealizada"] = true;
                    return RedirectToAction("Index", "Home");
                }
            }
            else {
                TempData["peticionEntradaRealizadaRepetida"] = true;
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: EntradaMiembros/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entradaMiembro = await _context.EntradaMiembro.FindAsync(id);
            if (entradaMiembro == null)
            {
                return NotFound();
            }
            ViewData["IdEntrada"] = new SelectList(_context.Entrada, "Id", "Titulo", entradaMiembro.IdEntrada);
            ViewData["IdMiembro"] = new SelectList(_context.Miembro, "Id", "Apellido", entradaMiembro.IdMiembro);
            return View(entradaMiembro);
        }

        // POST: EntradaMiembros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EntradaMiembro entradaMiembro)
        {
            if (id != entradaMiembro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entradaMiembro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaMiembroExists(entradaMiembro.Id))
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
            ViewData["IdEntrada"] = new SelectList(_context.Entrada, "Id", "Titulo", entradaMiembro.IdEntrada);
            ViewData["IdMiembro"] = new SelectList(_context.Miembro, "Id", "Apellido", entradaMiembro.IdMiembro);
            return View(entradaMiembro);
        }

        // GET: EntradaMiembros/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entradaMiembro = await _context.EntradaMiembro
                .Include(e => e.Entrada)
                .Include(e => e.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entradaMiembro == null)
            {
                return NotFound();
            }

            return View(entradaMiembro);
        }

        // POST: EntradaMiembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entradaMiembro = await _context.EntradaMiembro.FindAsync(id);
            _context.EntradaMiembro.Remove(entradaMiembro);
            await _context.SaveChangesAsync();
            var idMiembroActual = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToAction("Index", "EntradaMiembros", new { id = idMiembroActual });
        }

        private bool EntradaMiembroExists(Guid? id)
        {
            return _context.EntradaMiembro.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult CambiarEstadoEntradaMiembro(Guid id)
        {
            EntradaMiembro emAux = new EntradaMiembro();

            if (EntradaMiembroExists(id))
            {
                emAux = _context.EntradaMiembro.Find(id);
            }

            if (emAux.Habilitado)
            {
                emAux.Habilitado = false;
            }
            else
            {
                emAux.Habilitado = true;
            }

            _context.Update(emAux);
            _context.SaveChanges();
            var idMiembroActual = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return RedirectToAction("Index", "EntradaMiembros", new { id = idMiembroActual });

        }
    }
}
