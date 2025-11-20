using DotNetEnv;
using GrapheneTrace.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load .env variables if present
DotNetEnv.Env.Load();

// Read from environment variables
var connectionString = $"server={Environment.GetEnvironmentVariable("MYSQL_HOST")};" +
                       $"port={Environment.GetEnvironmentVariable("MYSQL_PORT")};" +
                       $"user={Environment.GetEnvironmentVariable("MYSQL_USER")};" +
                       $"password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};" +
                       $"database={Environment.GetEnvironmentVariable("MYSQL_DATABASE")}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
