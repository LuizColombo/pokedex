using pokedex.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddScoped<PokedexService>();
builder.Services.AddScoped<PokedexHabitatService>();
builder.Services.AddScoped<PokedexTypeService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pokedex}/{action=Index}/{id?}");

app.Run();
