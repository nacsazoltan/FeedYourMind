using System.Globalization;
using Microsoft.AspNetCore.Localization;
using OkosDobozWeb.Services; // We will create this service in Step 4

var builder = WebApplication.CreateBuilder(args);

// 1. Add services for localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// 2. Add our custom service for content
builder.Services.AddSingleton<ContentService>(); // Add our mock content service

// 2b. Add SQL connection factory (Azure SQL / SQL Server)
builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

// 2c. Data repositories
builder.Services.AddSingleton<IVideoRepository, VideoRepository>();
builder.Services.AddSingleton<IExerciseRepository, ExerciseRepository>();
builder.Services.AddSingleton<IResultsRepository, ResultsRepository>();
builder.Services.AddSingleton<IPageRepository, PageRepository>();

// 3. Configure localization options
var supportedCultures = new[]
{
    new CultureInfo("hu"), // Hungarian (Default)
    new CultureInfo("en"), // English
    new CultureInfo("cs")  // Czech
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("hu");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    
    // Use cookie provider first, then fall back to Accept-Language header
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 4. Use the localization middleware - MUST be before routing
app.UseRequestLocalization();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}");

app.Run();