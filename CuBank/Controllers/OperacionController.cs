using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuBank.DAO;
using CuBank.Models;
using System.Security.Cryptography;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Globalization;

namespace CuBank.Controllers
{
    public class OperacionController : Controller
    {
        private readonly MyContext _context;

        public static int saldoActual = 1000;

        public static string mensaje = ""; 

        public OperacionController(MyContext context)
        {
            _context = context;
        }

        [SessionCheck]
        [Route("accounts/{UserId}")]
        public IActionResult Index(int UserId)
        {
            ViewBag.NombreCompleto = $"{HttpContext.Session.GetString("Nombre")} {HttpContext.Session.GetString("Apellido")}";

            ViewBag.Saldo = saldoActual;

            ViewBag.Error = mensaje;

            var userId = HttpContext.Session.GetInt32("Id");

            var usuario = _context.Usuarios.Include(c => c.TotalOperaciones).Where(u => u.UsuarioId == UserId).FirstOrDefault();

            if (usuario != null)
            {
                if (usuario.UsuarioId != userId)
                {
                    return RedirectToAction("Logout", "Usuario");
                }
                else
                {
                    MyViewModel myModel = new MyViewModel()
                    {
                        OperacionUsuario = _context.Operaciones.Include(c => c.User).Where(u => u.UsuarioId == UserId).FirstOrDefault(),
                        ListaHistorial = _context.Operaciones.Include(c => c.User).Where(u => u.UsuarioId == UserId).ToList()
                    };

                    return View(myModel);
                }
            }
            else
            {
                return RedirectToAction("Logout", "Usuario");
            }
        }

        [HttpGet]
        [HttpPost]
        public IActionResult MostrarIdentificador()
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));

            return RedirectToAction("Index", "Operacion", new { userId });
        }

        [HttpGet]
        [HttpPost]
        public IActionResult AgregarOperacion(Operacion operacion)
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));

            int num = 0;

            if (operacion.Monto != 0)
            {
                if (ModelState.IsValid)
                {
                    operacion.UsuarioId = userId;

                    if (operacion.Monto < 0)
                    {
                        num = operacion.Monto * -1;
                    }

                    if (saldoActual < num)
                    {
                        mensaje = "El Saldo no puede ser menor que cero";
                        
                        return RedirectToAction("Index", "Operacion", new { userId });
                    }
                    else
                    {
                        mensaje = "";
                        _context.Add(operacion);
                        _context.SaveChanges();

                        saldoActual += operacion.Monto;

                        return RedirectToAction("Index", "Operacion", new { userId });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Operacion", new { userId });
                }
            }
            else
            {
                mensaje = "";

                return RedirectToAction("Index", "Operacion", new { userId });
            }
        }

        [HttpGet]
        [HttpPost]
        public IActionResult HistorialMonto()
        {
            return View();
        }

        private bool OperacionExists(int id)
        {
            return (_context.Operaciones?.Any(e => e.OperacionId == id)).GetValueOrDefault();
        }
    }
}
