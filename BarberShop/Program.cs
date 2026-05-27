using Microsoft.EntityFrameworkCore;
using BarberShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Registra os controllers e views MVC
builder.Services.AddControllersWithViews();

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.UseStaticWebAssets();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// UseStaticFiles serve os arquivos da pasta wwwroot/ (css, js, lib, imagens...)
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
