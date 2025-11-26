using MAlex;
using MAlex.Services;
using MetroApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
// 1?? Add services
// -------------------------------
builder.Services.AddControllersWithViews();

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true; // require email confirmation
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders(); // Important: register token providers

// Register EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// -------------------------------
// 2?? Configure middleware
// -------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Identity authentication
app.UseAuthorization();

// -------------------------------
// 3?? Configure endpoints
// -------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
