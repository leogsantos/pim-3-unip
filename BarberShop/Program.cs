using Microsoft.EntityFrameworkCore;
using BarberShop.Data;
using BarberShop.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Registra os controllers e views MVC
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
    });

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

// Aponta o logger para a pasta logs/ dentro da raiz do projeto
AppLogger.Init(app.Environment.ContentRootPath);

AppLogger.Info($"Ambiente: {app.Environment.EnvironmentName}");
AppLogger.Info($"Banco de dados: {builder.Configuration.GetConnectionString("DefaultConnection")}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    AppLogger.Info("Modo: PRODUÇÃO (HSTS habilitado)");
}
else
{
    AppLogger.Info("Modo: DESENVOLVIMENTO");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

AppLogger.Info("=== APLICAÇÃO PRONTA PARA RECEBER REQUISIÇÕES ===");

app.Run();
