using Ecommerce_Website.Data;
using Ecommerce_Website.Repositories;
using Ecommerce_Website.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;
using Ecommerce_Website.Core.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount =true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();    

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IRepository<CategoryModel>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IRepository<Borshor>, BorshorRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRepository<Testmonials>,TestmonialsRepository >();
builder.Services.AddScoped<IRepository<ApplicationUser>, UserRepository>();
builder.Services.AddScoped<IRepository<Order>, OrdersRepository>();
builder.Services.AddScoped<IOrderRepository, OrdersRepository>();
builder.Services.AddScoped<IRepository<Blog>, BlogRepository>();

builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            return factory.Create(typeof(JsonStringLocalizer));
        };
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ar-EG"),
        new CultureInfo("en-US"),
    };
    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0] );
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddSession();





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
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

var supportedCultures = new[] { "ar-EG", "en-US" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
 
app.UseRequestLocalization(localizationOptions);



app.UseAuthorization();
//-----------------------//
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var scope = scopeFactory.CreateScope();

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


await DefaultRoles.SeedRolesAsync(roleManager);
await DefaultUsers.SeedAdminUserAsync(userManager);
//-----------------------//

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

//-----------------------//
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    await next();
});
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    await next();
});

app.UseHsts();
