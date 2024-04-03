using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sips.Data;
using Sips.Data.Services;
using Sips.Services;
using Sips.SipsModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<SipsdatabaseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});
// Add HttpClient and PayPalTokenService
builder.Services.AddHttpClient();

// Register PayPalTokenService as a transient service
builder.Services.AddTransient<PayPalTokenService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var clientId = configuration["PayPal:ClientId"]; // Assuming your configuration follows the format "PayPal:ClientId"
    var clientSecret = configuration["PayPal:ClientSecret"]; // Assuming your configuration follows the format "PayPal:ClientSecret"

    return new PayPalTokenService(httpClientFactory.CreateClient(), configuration);
});



builder.Services.AddTransient<IEmailService, EmailService>();

//builder.Services.AddSession();


var app = builder.Build();

app.UseSession();

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

//handle error
app.UseExceptionHandler("/Error");

//error handling
app.UseStatusCodePagesWithReExecute("/Error/StatusCodePage", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
