using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevamsizlikTakip.Data;
using DevamsizlikTakip.Models;

namespace DevamsizlikTakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly DevamsizlikContext _context;

        public AccountController(DevamsizlikContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.TCKimlik == model.TCKimlik);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", $"{user.Ad} {user.Soyad}");
                    HttpContext.Session.SetString("UserType", user.KullaniciTipi.ToString());

                    // Kullanıcı tipine göre yönlendirme
                    if (user.KullaniciTipi == UserType.Admin)
                    {
                        TempData["SuccessMessage"] = $"Hoş geldiniz, {user.Ad} {user.Soyad}!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["SuccessMessage"] = $"Hoş geldiniz, {user.Ad} {user.Soyad}! Yeni devamsızlık kaydı oluşturabilirsiniz.";
                        return RedirectToAction("Create", "Devamsizlik");
                    }
                }

                ModelState.AddModelError("", "E-posta veya TC Kimlik No hatalı!");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
} 