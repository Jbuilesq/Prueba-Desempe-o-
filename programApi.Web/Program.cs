using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using programApi.Infrastructure.Data;   // nuestros contextos

var builder = WebApplication.CreateBuilder(args);

// ---------- MySQL ----------
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseMySql(connectionString, MySqlServerVersion.AutoDetect(connectionString)));

//para usar mas adelante AppDbContext en Web
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, MySqlServerVersion.AutoDetect(connectionString)));
// ---------------------------------------------------

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity sobre IdentityDbContext
builder.Services.AddDefaultIdentity<IdentityUser<int>>(opts => opts.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
