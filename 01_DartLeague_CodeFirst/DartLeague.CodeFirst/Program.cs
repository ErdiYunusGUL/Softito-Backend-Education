using DartLeague.CodeFirst.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Ensure migrations are applied
    context.Database.Migrate();

    var newCategory = context.Categories.FirstOrDefault(c => c.Name == "istanbul dart ligi");
    if (newCategory == null)
    {
        newCategory = new Category { Name = "istanbul dart ligi" };
        context.Categories.Add(newCategory);
        context.SaveChanges();
    }

    var newVenue = context.Venues.FirstOrDefault(v => v.Name == "Out Pub");
    if (newVenue == null)
    {
        newVenue = new Venue { Name = "Out Pub" };
        context.Venues.Add(newVenue);
        context.SaveChanges();
    }

    var newTeam = context.Teams.FirstOrDefault(t => t.Name == "Idles");
    if (newTeam == null)
    {
        newTeam = new Team { Name = "Idles", CategoryId = newCategory.Id, HomeVenueId = newVenue.Id };
        context.Teams.Add(newTeam);
        context.SaveChanges();
    }
}

app.Run();
