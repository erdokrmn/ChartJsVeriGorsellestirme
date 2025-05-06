using ChartJsVeriGorsellestirme.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC Controller + View desteðini aktif et
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


var app = builder.Build();

// wwwroot içindeki statik dosyalar için (JS, CSS vs.)
app.UseStaticFiles();

// Routing sistemi devreye alýnýr
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chart}/{action=Index}/{id?}");

app.Run();
