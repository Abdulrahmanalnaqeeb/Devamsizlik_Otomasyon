using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevamsizlikTakip.Data;
using DevamsizlikTakip.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DevamsizlikTakip.Controllers
{
    public class DevamsizlikController : Controller
    {
        private readonly DevamsizlikContext _context;

        public DevamsizlikController(DevamsizlikContext context)
        {
            _context = context;
        }

        // Devamsızlık Listesi
        public async Task<IActionResult> Index()
        {
            // Get current user info
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Sadece yöneticiler listeyi görebilir
            if (userType != UserType.Admin.ToString())
            {
                return RedirectToAction("Create");
            }

            var devamsizliklar = await _context.Devamsizliklar
                .Include(d => d.User)
                .OrderByDescending(d => d.BaslangicTarihi)
                .ToListAsync();

            return View(devamsizliklar);
        }

        // Yeni Devamsızlık Kaydı
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Yönetici tüm kullanıcıları görebilir, diğerleri sadece kendilerini
            var usersQuery = _context.Users.AsQueryable();
            if (userType != UserType.Admin.ToString())
            {
                usersQuery = usersQuery.Where(u => u.Id == userId);
            }

            ViewBag.Users = new SelectList(
                usersQuery.OrderBy(u => u.Ad)
                    .Select(u => new
                    {
                        Id = u.Id,
                        AdSoyad = $"{u.Ad} {u.Soyad}"
                    }),
                "Id",
                "AdSoyad"
            );

            ViewBag.DevamsizlikTurleri = new SelectList(
                Enum.GetValues(typeof(DevamsizlikTuru))
                    .Cast<DevamsizlikTuru>()
                    .Select(e => new
                    {
                        Id = e,
                        Name = e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? e.ToString()
                    }),
                "Id",
                "Name"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,BaslangicTarihi,BitisTarihi,DevamsizlikTipi,Aciklama")] Devamsizlik devamsizlik)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Yönetici değilse sadece kendi için kayıt yapabilir
                if (userType != UserType.Admin.ToString() && devamsizlik.UserId != userId)
                {
                    ModelState.AddModelError("", "Sadece kendiniz için devamsızlık kaydı oluşturabilirsiniz.");
                }
                else if (devamsizlik.BitisTarihi < devamsizlik.BaslangicTarihi)
                {
                    ModelState.AddModelError("BitisTarihi", "Bitiş tarihi başlangıç tarihinden önce olamaz");
                }
                else
                {
                    devamsizlik.Onaylandi = false;
                    _context.Add(devamsizlik);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Devamsızlık kaydınız başarıyla oluşturuldu.";
                    
                    // Yönetici ise listeye, değilse yeni kayıt sayfasına yönlendir
                    if (userType == UserType.Admin.ToString())
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction(nameof(Create));
                    }
                }
            }

            // Hata durumunda ViewBag'leri yeniden doldur
            var usersQuery = _context.Users.AsQueryable();
            if (userType != UserType.Admin.ToString())
            {
                usersQuery = usersQuery.Where(u => u.Id == userId);
            }

            ViewBag.Users = new SelectList(
                usersQuery.OrderBy(u => u.Ad)
                    .Select(u => new
                    {
                        Id = u.Id,
                        AdSoyad = $"{u.Ad} {u.Soyad}"
                    }),
                "Id",
                "AdSoyad",
                devamsizlik.UserId
            );

            ViewBag.DevamsizlikTurleri = new SelectList(
                Enum.GetValues(typeof(DevamsizlikTuru))
                    .Cast<DevamsizlikTuru>()
                    .Select(e => new
                    {
                        Id = e,
                        Name = e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? e.ToString()
                    }),
                "Id",
                "Name",
                devamsizlik.DevamsizlikTipi
            );

            return View(devamsizlik);
        }

        // Diğer action'lar sadece admin için erişilebilir olmalı
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("UserType") != UserType.Admin.ToString())
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var devamsizlik = await _context.Devamsizliklar.FindAsync(id);
            if (devamsizlik == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(
                _context.Users.OrderBy(u => u.Ad)
                    .Select(u => new
                    {
                        Id = u.Id,
                        AdSoyad = $"{u.Ad} {u.Soyad}"
                    }),
                "Id",
                "AdSoyad",
                devamsizlik.UserId
            );

            ViewBag.DevamsizlikTurleri = new SelectList(
                Enum.GetValues(typeof(DevamsizlikTuru))
                    .Cast<DevamsizlikTuru>()
                    .Select(e => new
                    {
                        Id = e,
                        Name = e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? e.ToString()
                    }),
                "Id",
                "Name",
                devamsizlik.DevamsizlikTipi
            );

            return View(devamsizlik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BaslangicTarihi,BitisTarihi,DevamsizlikTipi,Aciklama,Onaylandi")] Devamsizlik devamsizlik)
        {
            if (HttpContext.Session.GetString("UserType") != UserType.Admin.ToString())
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != devamsizlik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devamsizlik);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Devamsızlık kaydı başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevamsizlikExists(devamsizlik.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Users = new SelectList(
                _context.Users.OrderBy(u => u.Ad)
                    .Select(u => new
                    {
                        Id = u.Id,
                        AdSoyad = $"{u.Ad} {u.Soyad}"
                    }),
                "Id",
                "AdSoyad",
                devamsizlik.UserId
            );

            ViewBag.DevamsizlikTurleri = new SelectList(
                Enum.GetValues(typeof(DevamsizlikTuru))
                    .Cast<DevamsizlikTuru>()
                    .Select(e => new
                    {
                        Id = e,
                        Name = e.GetType()
                            .GetMember(e.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? e.ToString()
                    }),
                "Id",
                "Name",
                devamsizlik.DevamsizlikTipi
            );

            return View(devamsizlik);
        }

        // Devamsızlık Onaylama
        [HttpPost]
        public async Task<IActionResult> Onayla(int id)
        {
            var devamsizlik = await _context.Devamsizliklar.FindAsync(id);
            if (devamsizlik == null)
                return NotFound();

            devamsizlik.Onaylandi = true;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Devamsızlık kaydı onaylandı!";
            return RedirectToAction(nameof(Index));
        }

        // Devamsızlık Detayı
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var devamsizlik = await _context.Devamsizliklar
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (devamsizlik == null)
                return NotFound();

            return View(devamsizlik);
        }

        // Silme işlemi için GET action
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var devamsizlik = await _context.Devamsizliklar
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (devamsizlik == null)
                return NotFound();

            return View(devamsizlik);
        }

        // Silme işlemi için POST action
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devamsizlik = await _context.Devamsizliklar.FindAsync(id);
            if (devamsizlik != null)
            {
                _context.Devamsizliklar.Remove(devamsizlik);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Devamsızlık kaydı başarıyla silindi!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DevamsizlikExists(int id)
        {
            return _context.Devamsizliklar.Any(e => e.Id == id);
        }
    }
}