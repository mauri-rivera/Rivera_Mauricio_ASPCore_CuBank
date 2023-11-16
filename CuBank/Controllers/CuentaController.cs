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
    public class CuentaController : Controller
    {
        private readonly MyContext _context;

        public CuentaController(MyContext context)
        {
            _context = context;
        }

        [SessionCheck]
        [Route("accounts/{UsuarioId}")]
        public IActionResult Index(int UsuarioId)
        {
            List<Cuenta> listaCuenta = _context.Cuentas.Include(c => c.User).Where(c => c.UsuarioId == UsuarioId).ToList();
            return View(listaCuenta);
        }

        [SessionCheck]
        [HttpGet]
        [HttpPost]
        public IActionResult ActualizarMonto()
        {
            return null;
        }

        private bool CuentaExists(int id)
        {
          return (_context.Cuentas?.Any(e => e.CuentaId == id)).GetValueOrDefault();
        }
    }
}
