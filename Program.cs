using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthSystem.Data;
using AuthSystem.Areas.Identity.Data;
using AuthSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Bağlantı dizesini al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// AuthDbContext'i ekle
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity ayarlarını Teacher sınıfı ile güncelleyin
builder.Services.AddIdentity<Teacher, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// Diğer servisleri ekle
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// HTTP istek boru hattını yapılandır
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Kimlik doğrulamayı ekleyin
app.UseAuthorization();

// Route yapılandırması
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Uygulamayı çalıştır
app.Run();