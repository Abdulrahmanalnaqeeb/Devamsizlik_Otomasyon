using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevamsizlikTakip.Data;
using DevamsizlikTakip.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DevamsizlikTakip.Controllers
{
    public class IstatistikController : Controller
    {
        private readonly DevamsizlikContext _context;

        public IstatistikController(DevamsizlikContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Toplam kullanıcı sayıları
            var kullaniciSayilari = await _context.Users
                .GroupBy(u => u.KullaniciTipi)
                .Select(g => new { Tip = g.Key, Sayi = g.Count() })
                .ToListAsync();

            // Devamsızlık türlerine göre dağılım
            var devamsizlikDagilimi = await _context.Devamsizliklar
                .GroupBy(d => d.DevamsizlikTipi)
                .Select(g => new { Tur = g.Key, Sayi = g.Count() })
                .ToListAsync();

            // Onay durumuna göre devamsızlıklar
            var onayDurumu = await _context.Devamsizliklar
                .GroupBy(d => d.Onaylandi)
                .Select(g => new { Onaylandi = g.Key, Sayi = g.Count() })
                .ToListAsync();

            // En çok devamsızlığı olan kullanıcılar (ilk 5)
            var enCokDevamsizlik = await _context.Users
                .Select(u => new
                {
                    Kullanici = u,
                    DevamsizlikSayisi = u.Devamsizliklar.Count
                })
                .OrderByDescending(x => x.DevamsizlikSayisi)
                .Take(5)
                .ToListAsync();

            ViewBag.KullaniciSayilari = kullaniciSayilari;
            ViewBag.DevamsizlikDagilimi = devamsizlikDagilimi;
            ViewBag.OnayDurumu = onayDurumu;
            ViewBag.EnCokDevamsizlik = enCokDevamsizlik;

            return View();
        }
    }
} 