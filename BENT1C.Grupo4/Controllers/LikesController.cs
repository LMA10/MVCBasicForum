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
    public class LikesController : Controller
    {
        private readonly ForoDbContext _context;

        public LikesController(ForoDbContext context)
        {
            _context = context;
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var foroDbContext = _context.Like.Include(l => l.Miembro).Include(l => l.Respuesta);
            return View(await foroDbContext.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Like
                .Include(l => l.Miembro)
                .Include(l => l.Respuesta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // GET: Likes/Create
        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido");
            ViewData["RespuestaId"] = new SelectList(_context.Respuesta, "Id", "Descripcion");
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid RespuestaId, Guid MiembroId, bool meGusta, Guid idEntrada, Guid idLike)
        {
            Like like = new Like();
            
            var existeLike = _context.Like.Any(r => r.MiembroId == MiembroId && r.RespuestaId == RespuestaId);

            if (!existeLike || idLike != Guid.Empty)
            {
                if (idLike != Guid.Empty)
                {
                    await DeleteConfirmed(idLike, idEntrada);
                }
                else 
                {
                    if (ModelState.IsValid)
                    {
                        like.RespuestaId = RespuestaId;
                        like.MiembroId = MiembroId;
                        like.MeGusta = meGusta;
                        like.Fecha = DateTime.Now;
                        like.Id = Guid.NewGuid();
                        _context.Add(like);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Details", "Entradas", new { id = idEntrada });
                    }
                }
            }
            else 
            {
                TempData["validaLike"] = true;
            }
            //ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", like.MiembroId);
            //ViewData["RespuestaId"] = new SelectList(_context.Respuesta, "Id", "Descripcion", like.RespuestaId);
            return RedirectToAction("Details", "Entradas", new { id = idEntrada });
        }

        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = await _context.Like.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", like.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuesta, "Id", "Descripcion", like.RespuestaId);
            return View(like);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MeGusta,RespuestaId,MiembroId")] Like like)
        {
            if (id != like.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    like.Fecha = DateTime.Now;
                    _context.Update(like);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikeExists(like.Id))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembro, "Id", "Apellido", like.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuesta, "Id", "Descripcion", like.RespuestaId);
            return View(like);
        }

        // GET: Likes/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var like = _context.Like
                .Include(l => l.Miembro)
                .Include(l => l.Respuesta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (like == null)
            {
                return NotFound();
            }

            return View(like);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid idEntrada)
        {
            var like = await _context.Like.FindAsync(id);
            _context.Like.Remove(like);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Entradas", new { id = idEntrada });
        }

        private bool LikeExists(Guid id)
        {
            return _context.Like.Any(e => e.Id == id);
        }
    }
}
