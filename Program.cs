using GrapheneTrace.Data;              // Your EF Core DbContext namespace
using Microsoft.EntityFrameworkCore;   // EF Core
using PatientDashboard.Hubs;           // Your SignalR Hub namespace

var builder = WebApplication.CreateBuilder(args);

// 🔹 Database Connection (MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "server=localhost;database=graphenetrace;user=root;password=your_password";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// 🔹 MVC Controllers + Views
builder.Services.AddControllersWithViews();

// 🔹 SignalR (for real-time chat)
builder.Services.AddSignalR();

var app = builder.Build();

// 🔹 Error Handling & Environment Settings
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection(); // optional, enable if you’re using HTTPS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 🔹 Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Dashboard}/{id?}");

// 🔹 Map SignalR Hub route
app.MapHub<ChatHub>("/chathub");

app.Run();
