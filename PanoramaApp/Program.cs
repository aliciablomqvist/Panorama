using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Services;
using PanoramaApp.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<ReviewService>(); // For reviews

// Loggning
builder.Logging.ClearProviders();
builder.Logging.AddConsole();




var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.MapHub<ChatHub>("/chatHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<ChatHub>("/chathub"); // Map SignalR hub
});
app.MapRazorPages();
app.Run();
