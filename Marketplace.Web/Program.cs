using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Marketplace.Web.Data;
using Marketplace.Modules.Listings.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Marketplace.Web.Services.Listing;
using Marketplace.Web.Options;
using Marketplace.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<GoogleMapsOptions>(
    builder.Configuration.GetSection("GoogleMaps"));

builder.Services.AddListingsModule(builder.Configuration);

builder.Services.AddScoped<IListingCatalogService, ListingCatalogService>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("uk")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var localizationOptions =
    app.Services.GetRequiredService<
        Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value;

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Listings}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "services",
    pattern: "services",
    defaults: new { controller = "Catalog", action = "Index" });

app.MapControllerRoute(
    name: "listing-details",
    pattern: "{city}/{category}/{subcategory}/{slug}-{id:int}",
    defaults: new { controller = "Listings", action = "Details" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "subcategory",
    pattern: "{city}/{category}/{subcategory}",
    defaults: new { controller = "Catalog", action = "Subcategory" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "category",
    pattern: "{city}/{category}",
    defaults: new { controller = "Catalog", action = "Category" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "city",
    pattern: "{city}",
    defaults: new { controller = "Catalog", action = "City" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
