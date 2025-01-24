using Microsoft.EntityFrameworkCore;
using DevamsizlikTakip.Data;
using DevamsizlikTakip.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS politikasını ekle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7001", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DbContext yapılandırması
builder.Services.AddDbContext<DevamsizlikContext>(options =>
    options.UseSqlServer("Server=DESKTOP-6TV75J4\\SQLEXPRESS;Database=Devamsizlik;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"));

builder.Services.AddControllersWithViews();

// Session desteği ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// CORS'u etkinleştir
app.UseCors();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSession(); // UseRouting'den önce ekleyin

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
