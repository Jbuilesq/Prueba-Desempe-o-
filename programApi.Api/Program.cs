using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using programApi.Application.Interface;
using programApi.Application.Interfaces;
using programApi.Application.Services;
using programApi.Domain.Interfaces;
using programApi.Infrastructure.Data;
using programApi.Infrastructure.Repositories;
using programApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionStrings = 
    $"Server={Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost"};" +
    $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "3306"};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME") ?? "talentoplus"};" +
    $"Uid={Environment.GetEnvironmentVariable("DB_USER") ?? "root"};" +
    $"Pwd={Environment.GetEnvironmentVariable("DB_PASS") ?? "root"};";

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
jwtSettings["Secret"]  = Environment.GetEnvironmentVariable("JWT_SECRET") ?? jwtSettings["Secret"];
jwtSettings["Issuer"]  = Environment.GetEnvironmentVariable("JWT_ISSUER")  ?? jwtSettings["Issuer"];
jwtSettings["Audience"]= Environment.GetEnvironmentVariable("JWT_AUDIENCE")?? jwtSettings["Audience"];
jwtSettings["ExpiryMinutes"] = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES") ?? "60";
jwtSettings["RefreshExpiryDays"] = Environment.GetEnvironmentVariable("JWT_REFRESH_DAYS") ?? "7";




// ---------- MySQL ----------
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, MySqlServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseMySql(connectionString, MySqlServerVersion.AutoDetect(connectionString)));
// ----------------------------

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TalentoPlus API", Version = "v1" });

    // Definir esquema JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa el token JWT con el prefijo **Bearer**",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Aplicar seguridad global (opcional: puedes filtrar después)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// ------------------------------

// ---------- JWT ----------
var jwtSettings1 = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings1["Secret"] ?? "talentoPlusSuperSecretKey";
var issuer = jwtSettings1["Issuer"] ?? "talentoplus";
var audience = jwtSettings1["Audience"] ?? "talentoplusemployees";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
// ----------------------------

// ---------- Identity ----------
builder.Services.AddIdentityCore<IdentityUser<int>>()
    .AddRoles<IdentityRole<int>>()   // <-- agrega esto
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();
// ------------------------------

// ---------- Authorization ----------
builder.Services.AddAuthorization();
// ---------------------------------

// ---------- Controllers ----------
builder.Services.AddControllers();
// ---------------------------------

// ---------- Services ----------
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
// ------------------------------------------

var app = builder.Build();

// ---------- Pipeline ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



// ---------- Seed roles ----------
using var scope = app.Services.CreateScope();
var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<int>>>();
await SeedRolesAsync(roleMgr, userMgr);

app.Run();


static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleMgr,
    UserManager<IdentityUser<int>> userMgr)
{
    if (!await roleMgr.RoleExistsAsync("Admin"))
        await roleMgr.CreateAsync(new IdentityRole<int>("Admin"));
    if (!await roleMgr.RoleExistsAsync("User"))
        await roleMgr.CreateAsync(new IdentityRole<int>("User"));

    // Admin por defecto
    //c
    var admin = await userMgr.FindByEmailAsync("admin@talentoplus.com");
    if (admin is null)
    {
        admin = new IdentityUser<int> { UserName = "admin@talentoplus.com", Email = "admin@talentoplus.com" };
        await userMgr.CreateAsync(admin, "Admin123*");
        await userMgr.AddToRoleAsync(admin, "Admin");
    }
}