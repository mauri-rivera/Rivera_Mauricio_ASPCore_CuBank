using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuBank.DAO;
using CuBank.Models;

namespace CuBank.Controllers
{
    public class OperacionController : Controller
    {
        private readonly MyContext _context;

        public OperacionController(MyContext context)
        {
            _context = context;
        }

        [SessionCheck]
        [Route("accounts/{OperacionId}")]
        public IActionResult Index(int OperacionId)
        {
            MyViewModel MyModels = new MyViewModel
            {
                OperacionUsuario = _context.Operaciones.Include(c => c.User).Where(c => c.OperacionId == OperacionId).First(),
                ListaHistorial = _context.Registros.ToList()
            };

            return View(MyModels);
        }

        //[SessionCheck]
        [HttpGet]
        [HttpPost]
        public IActionResult AgregarOperacion(Operacion Operacion)
        {
            Operacion.UsuarioId = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));

            var valorId = _context.Operaciones.Include(c => c.User).FirstOrDefault(s => s.UsuarioId == Operacion.UsuarioId);

            if (valorId != null)
            {
                return RedirectToAction("Index", "Operacion", new { Operacion.OperacionId });
            }
            else
            {
                Operacion.Monto = 1000;

                if (ModelState.IsValid)
                {
                    _context.Add(Operacion);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Operacion", new { Operacion.OperacionId });
                }
                else
                {
                    return View("Index", "Operacion");
                }
            }
        }

        [SessionCheck]
        [HttpGet]
        [HttpPost]
        public IActionResult HistorialMonto()
        {
            return null;
        }

        [SessionCheck]
        [HttpGet]
        [HttpPost]
        public IActionResult ActualizarMonto()
        {
            return null;
        }

        private bool OperacionExists(int id)
        {
            return (_context.Operaciones?.Any(e => e.OperacionId == id)).GetValueOrDefault();
        }
    }
}
