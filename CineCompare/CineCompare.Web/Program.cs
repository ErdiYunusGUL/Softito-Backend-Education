using CineCompare.Core.Interfaces;
using CineCompare.Data.Contexts;
using CineCompare.Data.Repositories;
using CineCompare.Data.UnitOfWorks;
using CineCompare.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Yapılandırması
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/cinecompare-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 1. Veritabanı
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<CineCompareDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity (Kimlik Doğrulama) Ayarları
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<CineCompareDbContext>()
.AddDefaultTokenProviders();

// 3. JWT Ayarları
var jwtKey = builder.Configuration["Jwt:Key"];
// DÜZELTME 1: Null durumunda çökmesini engellemek ve uyarı vermek için kontrol
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException("appsettings.json dosyasında 'Jwt:Key' değeri bulunamadı!");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // DÜZELTME 2: Identity'nin Cookie yönlendirmesini ezip 401 hatası dönmesini sağlar
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 4. Swagger (API Arayüzü)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CineCompare API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token'ınızı buraya girin. Örnek: Bearer eyJhbGci..."
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

// 5. Bağımlılık (DI) Kayıtları
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDapperRepository, DapperRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IAiService, AiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://127.0.0.1:3000") // React Vite portları
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// DÜZELTME 3: Sadece bir API geliştiriyorsanız Views'a gerek yoktur, AddControllers yeterlidir.
builder.Services.AddControllers();

var app = builder.Build();

// DÜZELTME 4: Swagger sadece geliştirme ortamında görülmelidir (Güvenlik için).
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CineCompare API v1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Serilog Request Logging (İstekleri loglamak için)
app.UseSerilogRequestLogging();

app.UseCors("AllowReactApp");

// ÖNEMLİ: Authentication (Kimlik Sorma) her zaman Authorization'dan (Yetki Kontrolü) önce yazılmalı!
app.UseAuthentication();
app.UseAuthorization();

// DÜZELTME 3.1: API projelerinde yönlendirme bu şekilde olmalıdır.
app.MapControllers();

app.Run();