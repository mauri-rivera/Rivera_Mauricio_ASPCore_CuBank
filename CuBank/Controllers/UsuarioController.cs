using CuBank.DAO;
using CuBank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CuBank.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly MyContext _context;

        public UsuarioController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("registro")]
        public IActionResult RegistroUsuario(Usuario nuevoUsuario)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<Usuario> Hasher = new PasswordHasher<Usuario>();
                nuevoUsuario.Password = Hasher.HashPassword(nuevoUsuario, nuevoUsuario.Password);
                _context.Usuarios.Add(nuevoUsuario);
                _context.SaveChanges();
                HttpContext.Session.SetString("Nombre", nuevoUsuario.Nombre);
                HttpContext.Session.SetString("Apellido", nuevoUsuario.Apellido);
                HttpContext.Session.SetString("Email", nuevoUsuario.Email);
                HttpContext.Session.SetInt32("Id", nuevoUsuario.UsuarioId);
                return RedirectToAction("Index", "Cuenta", new { nuevoUsuario.UsuarioId });
            }
            return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                Usuario? usuario = _context.Usuarios.FirstOrDefault(us => us.Email == login.EmailLogin);

                if (usuario != null)
                {
                    PasswordHasher<Login> Hasher = new PasswordHasher<Login>();
                    var result = Hasher.VerifyHashedPassword(login, usuario.Password, login.PasswordLogin);

                    if (result != 0)
                    {
                        HttpContext.Session.SetString("Nombre", usuario.Nombre);
                        HttpContext.Session.SetString("Apellido", usuario.Apellido);
                        HttpContext.Session.SetString("Email", usuario.Email);
                        HttpContext.Session.SetInt32("Id", usuario.UsuarioId);
                        return RedirectToAction("Index", "Cuenta", new { usuario.UsuarioId });
                    }
                }
                ModelState.AddModelError("PasswordLogin", "Credenciales incorrectas");
                return View("Index");
            }
            return View("Index");
        }
    }
}
