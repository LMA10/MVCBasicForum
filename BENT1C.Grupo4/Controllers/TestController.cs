using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BENT1C.Grupo4.Controllers
{
    //Controlador dedicado a generar pruebas rapida con cargas de datos.

    //IMPORTANTE: Eliminar antes de su puesta en produccion
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
