using Microsoft.AspNetCore.Mvc;

using ProyectoSenaLmour.Models;
using ProyectoSenaLmour.Data;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ProyectoSenaLmour.Controllers
{
	public class AccesoController : Controller
	{

        private readonly LmourContext _context;

        public AccesoController(LmourContext context)
        {
            _context = context;
        }

        public IActionResult Index()
		{
			return View();

		}

		[HttpPost]
		public async Task<IActionResult> Index(UsuarioF _usuarioF)
		{

			var usuarioF = ValidarUsuarioF(_usuarioF.Correo, _usuarioF.Contraseña);

			if (usuarioF != null)
			{
				var claims = new List<Claim> {
				 new Claim(ClaimTypes.Name, usuarioF.Nombres),
				 new Claim("Correo", usuarioF.Correo),
                 new Claim(ClaimTypes.Role, usuarioF.IdRolNavigation.NomRol),
            };

				var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ClaimsIdentity));



				return RedirectToAction("Index", "Home");
			}
			else
			{

				return View();
			}

		}

		public async Task<IActionResult> Salir()
		{

			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Index", "acceso");

		}


		//prueba

		public IActionResult Registrar()
		{
			return View("~/Views/Acceso/Registrar.cshtml");
		}

        //public IActionResult Registrar()
        //{
        //    return View("~/Views/Usuarios/Create.cshtml");
        //}

        public IActionResult Recuperar()
		{
			return View("~/Views/Acceso/Recuperar.cshtml");
		}

        public Usuario ValidarUsuarioF(string _correo, string _contraseña)
        {
            return _context.Usuarios
				.Where(item => item.Correo == _correo && item.Contraseña == _contraseña)
				.Include(u => u.IdRolNavigation)
				.FirstOrDefault();
        }




    }




}


