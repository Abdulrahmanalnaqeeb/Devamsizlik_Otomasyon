using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevamsizlikTakip.Data;
using DevamsizlikTakip.Models;

namespace DevamsizlikTakip.Controllers
{
    public class UserController : Controller
    {
        private readonly DevamsizlikContext _context;

        public UserController(DevamsizlikContext context)
        {
            _context = context;
        }

        // Kullanıcı Listesi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // Kullanıcı Detayı
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .Include(u => u.Devamsizliklar)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // Yeni Kullanıcı Oluşturma
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad,Soyad,TCKimlik,Email,Telefon,KullaniciTipi,Sinif,Departman")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla kaydedildi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    
                    ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                }
            }
            
            return View(user);
        }

        // Kullanıcı Düzenleme
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Soyad,TCKimlik,Email,Telefon,KullaniciTipi,Sinif,Departman")] User user)
        {
            if (id != user.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(user);
        }

        // Silme işlemi için GET action
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // Silme işlemi için POST action
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu: " + ex.Message;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
} 