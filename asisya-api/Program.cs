using asisya_api.Data;
using asisya_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using asisya_api.Models;
using EFCore.BulkExtensions;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddSingleton<JwtService>();

var key = Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_FOR_ASISYA_API_2026");

// Configuración JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 🚀 Configurar CORS para React (puerto local)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // el puerto de tu frontend React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🚦 Middlewares
app.UseCors(); // debe ir antes de UseAuthentication y UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Inicialización de datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Borra y crea la base
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();

    // Crear categorías
    var servidores = new Category { Name = "SERVIDORES", PhotoUrl = "" };
    var cloud = new Category { Name = "CLOUD", PhotoUrl = "" };
    db.Categories.AddRange(servidores, cloud);
    await db.SaveChangesAsync();

    Console.WriteLine("⏳ Generando lista de 100.000 productos...");

    var products = new List<Product>(100_000);
    for (int i = 1; i <= 100_000; i++)
    {
        products.Add(new Product
        {
            Name = $"Producto {i}",
            Description = $"Descripción del producto {i}",
            Price = 10 + (i % 100),
            UnitsInStock = 100 - (i % 100),
            Discontinued = false,
            CategoryId = (i % 2 == 0 ? servidores.Id : cloud.Id)
        });
    }

    Console.WriteLine("🚀 Insertando masivamente con BulkInsert...");
    await db.BulkInsertAsync(products);


}

app.Run();
