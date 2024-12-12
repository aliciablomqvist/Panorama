// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PanoramaApp.Data;
using PanoramaApp.Hubs;
using PanoramaApp.Interfaces;
using PanoramaApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Dependency Injections
builder.Services.AddScoped<GroupChatService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddHttpClient<TmdbService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieCalendarService, MovieCalendarService>();
builder.Services.AddScoped<IMovieListService, MovieListService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUrlHelperService, UrlHelperService>();
builder.Services.AddScoped<IMovieListService, MovieListService>();


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

// app.MapRazorPages();
app.Run();