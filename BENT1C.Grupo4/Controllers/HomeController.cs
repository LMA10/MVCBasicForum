using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BENT1C.Grupo4.Models;
using BENT1C.Grupo4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;

namespace BENT1C.Grupo4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForoDbContext _context;

        public HomeController(ILogger<HomeController> logger, ForoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(CreateListToShow());
        }

        public IActionResult ShowEntriesByCategory(string nombre)
        {
            return View("Index",CreateListToShowByCategory(nombre));
        }

        private IEnumerable<Categoria> CreateListToShow() 
        {
            //metodos que devuelven informacion de las tablas para mostrar en la vista
            InitializeCategories();
            InitializeUsers();
            InitializeEntradas();

            PendingEntryRequest();

            var lista = _context.Categoria
                            .Include(c => c.Entradas)
                            .ThenInclude(p => p.Preguntas)
                            .ThenInclude(miembro => miembro.Miembro)
                            .Include(e => e.Entradas).ThenInclude(mh => mh.MiembrosHabilitados)
                            .ToList();

            return lista;
        }

        private IEnumerable<Categoria> CreateListToShowByCategory(string nombre)
        {
            InitializeCategories();
            InitializeUsers();
            InitializeEntradas();

            var lista = _context.Categoria
                            .Include(c => c.Entradas)
                            .ThenInclude(p => p.Preguntas)
                            .ThenInclude(r => r.Respuestas)
                            .ThenInclude(miembro => miembro.Miembro).Where(c => c.Nombre == nombre)
                            .Include(e => e.Entradas).ThenInclude(mh => mh.MiembrosHabilitados)
                            .ToList();

            ViewData["mostrarListaEntera"] = lista;

            return lista;
        }

        //Metodo que cuenta la cantidad de solicitudes pendientes del usuario logueado que no esten habilitadas
        public void PendingEntryRequest() {

            var miembroActualId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var cant = _context.EntradaMiembro.Include(e => e.Entrada)
                .ThenInclude(m => m.MiembrosHabilitados)
                .Where(a => a.Habilitado == false)
                .Count(c => c.Entrada.MiembroId == miembroActualId);
            ViewData["cantidadPendientes"] = cant;
        }

        public void InitializeCategories()
        {
            ViewData["listaCategorias"] = _context.Categoria.ToList();
        }
        public void InitializeEntradas()
        {
            ViewData["listaEntradas"] = _context.Entrada.Include(x => x.Preguntas).ThenInclude(u => u.Respuestas).ToList();
        }
        public void InitializeUsers()
        {
            ViewData["listaMiembros"] = _context.Miembro.Include(x => x.Entradas).ToList();
        }


        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult CreateEntry()
        {
            return RedirectToAction("Create", "Entradas");
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
