using System.Globalization;
using Marketplace.Modules.Listings.Infrastructure.DependencyInjection;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Modules.Blog.Infrastructure.DependencyInjection;
using Marketplace.Modules.Blog.Infrastructure.Persistence;
using Marketplace.Modules.Appointments.Infrastructure.DependencyInjection;
using Marketplace.Modules.Appointments.Infrastructure.Persistence;
using Marketplace.Modules.Notifications.Infrastructure.DependencyInjection;
using Marketplace.Modules.Notifications.Infrastructure.Persistence;
using Marketplace.Modules.Users.Infrastructure.DependencyInjection;
using Marketplace.Modules.Users.Infrastructure.Persistence;
using Marketplace.Web.Data;
using Marketplace.Web.Localization;
using Marketplace.Web.Mappings;
using Marketplace.Web.Navigation;
using Marketplace.Web.Options;
using Marketplace.Web.BackgroundJobs;
using Marketplace.Web.Seo;
using Marketplace.Web.Services.Catalog;
using Marketplace.Web.Services.ContactRequests;
using Marketplace.Web.Services.Home;
using Marketplace.Web.Services.Listings;
using Marketplace.Web.Services.Media;
using Marketplace.Web.Services.Seo;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UiOptions>(builder.Configuration.GetSection("Ui"));

builder.Services.Configure<LocationDefaultsOptions>(
    builder.Configuration.GetSection("LocationDefaults"));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("uk-UA"),
        new CultureInfo("ru-RU")
    };

    options.DefaultRequestCulture = new RequestCulture("uk-UA");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new RouteSegmentRequestCultureProvider()
    };
});

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddListingsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddNotificationsModule(builder.Configuration);
builder.Services.AddBlogModule(builder.Configuration);
builder.Services.AddAppointmentsModule(builder.Configuration);

builder.Services
    .AddDefaultIdentity<Microsoft.AspNetCore.Identity.IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<Microsoft.AspNetCore.Identity.IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICatalogVmMapper, CatalogVmMapper>();
builder.Services.AddScoped<IListingVmMapper, ListingVmMapper>();

builder.Services.AddScoped<ICatalogUrlBuilder, CatalogUrlBuilder>();
builder.Services.AddScoped<ICatalogBreadcrumbBuilder, CatalogBreadcrumbBuilder>();

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ISeoService, SeoService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<ICatalogFilterEnricher, CatalogFilterEnricher>();

builder.Services.AddScoped<ICatalogPaginationBuilder, CatalogPaginationBuilder>();

builder.Services.AddScoped<IContactRequestService, ContactRequestService>();
builder.Services.AddScoped<IPhotoStorageService, LocalPhotoStorageService>();

builder.Services.AddScoped<IAbsoluteUrlBuilder, AbsoluteUrlBuilder>();
builder.Services.AddScoped<CanonicalUrlBuilder>();
builder.Services.AddScoped<MetaBuilder>();
builder.Services.AddScoped<StructuredDataBuilder>();
builder.Services.AddScoped<SeoPaginationBuilder>();
builder.Services.AddScoped<SeoIndexingPolicy>();
builder.Services.AddScoped<HreflangBuilder>();

builder.Services.AddScoped<DbSeeder>();
builder.Services.AddScoped<AdminSeeder>();
builder.Services.AddScoped<AppointmentSeeder>();

builder.Services.AddHostedService<SubscriptionExpiryJob>();

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();

    var listingsDb = scope.ServiceProvider.GetRequiredService<ListingsDbContext>();
    await listingsDb.Database.MigrateAsync();

    var usersDb = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    await usersDb.Database.MigrateAsync();

    var notificationsDb = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
    await notificationsDb.Database.MigrateAsync();

    var blogDb = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    await blogDb.Database.MigrateAsync();

    var appointmentsDb = scope.ServiceProvider.GetRequiredService<AppointmentsDbContext>();
    await appointmentsDb.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();

    var appointmentSeeder = scope.ServiceProvider.GetRequiredService<AppointmentSeeder>();
    await appointmentSeeder.SeedAsync();
}

using (var scope = app.Services.CreateScope())
{
    var appDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await appDb.Database.MigrateAsync();

    var adminSeeder = scope.ServiceProvider.GetRequiredService<AdminSeeder>();
    await adminSeeder.SeedAsync();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Root}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();