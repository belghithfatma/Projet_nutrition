using app_nutri.Components;
using app_nutri.Data;
using app_nutri.Models;
using app_nutri.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "nutrition.db");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<StatisticsService>();

builder.Services.AddScoped<UserService>();

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<CustomAuthStateProvider>();

builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddScoped<AuthService>();

// Configure cookie authentication so the authorization middleware has an authentication service
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.Cookie.Name = "app_nutri_auth";
    });





var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Ingredients.Any())
    {
        db.Ingredients.AddRange(
            new Ingredient { Name = "Pâtes", Unit = "100 g", CaloriesPerUnit = 350 },
            new Ingredient { Name = "Sauce tomate", Unit = "100 g", CaloriesPerUnit = 80 },
            new Ingredient { Name = "Fromage", Unit = "100 g", CaloriesPerUnit = 400 },
            new Ingredient { Name = "Huile d’olive", Unit = "1 cuillère", CaloriesPerUnit = 120 }
        );
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

// Enable authentication/authorization middleware for secured endpoints
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
