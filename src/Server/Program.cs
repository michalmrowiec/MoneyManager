using MoneyManager.Server;
using MoneyManager.Server.Entities;
using MoneyManager.Server.Middlewaare;
using MoneyManager.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;

//var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Host.UseNLog();

//var authenticationSettings = new AuthenticationSettings();
//builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

//builder.Services.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = "Bearer";
//    option.DefaultScheme = "Bearer";
//    option.DefaultChallengeScheme = "Bearer";
//}).AddJwtBearer(cfg =>
//{
//    cfg.RequireHttpsMetadata = false;
//    cfg.SaveToken = true;
//    cfg.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = authenticationSettings.JwtIssuer,
//        ValidAudience = authenticationSettings.JwtIssuer,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
//    };
//});

//builder.Services.AddSingleton(authenticationSettings);
//builder.Services.AddDbContext<TrackerDbContext>();
//builder.Services.AddScoped<ITrackerService, TrackerService>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<IAccountService, AccountService>();
//builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//builder.Services.AddScoped<ErrorHandlingMiddleware>();
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();

// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseAuthentication();
//app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthorization();

//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

//app.Run();


public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseNLog();
}