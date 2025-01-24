using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevamsizlikTakip.Data;
using DevamsizlikTakip.Models;

namespace DevamsizlikTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DevamsizlikContext _context;

        public HomeController(ILogger<HomeController> logger, DevamsizlikContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Ana sayfaya doğrudan erişime izin ver
            return View();
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
