using SneakerDropPlatform.Data;
using SneakerDropPlatform.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Repository and DB Initializer
builder.Services.AddScoped<SneakerRepository>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<ReportsRepository>();
builder.Services.AddScoped<DbInitializer>();

var app = builder.Build();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    dbInitializer.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable Static Files for the Frontend
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

// Map Fallback to index.html for SPA-like behavior
app.MapFallbackToFile("index.html");

app.Run();
