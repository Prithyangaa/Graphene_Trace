using DotNetEnv;
using GrapheneTrace.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Load .env
Env.Load();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn))
);
=======
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

>>>>>>> b283525eb90ca6690cffbf8e55180a5495858727

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();

