using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoSenaLmour.Models;
using System.Diagnostics;

namespace ProyectoSenaLmour.Controllers
{
    public class HomeController : Controller
    {
        private readonly LmourContext _context;

        public HomeController(LmourContext context)
        {
            _context = context;
        }
      
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult DataPastel()
        {
            SeriePastel serie = new SeriePastel();
            return Json(serie.GetDataDummy());
        }

        public JsonResult DataBarras()
        {
            SerieBarra serie = new SerieBarra();
            return Json(serie.GetDataDummy());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}