using MediatR;
using MoneyManager.API.Middleware;
using MoneyManager.API.Services;
using MoneyManager.Application;
using MoneyManager.Application.Contracts.Persistence;
using MoneyManager.Infractructure;
using MoneyManager.Infractructure.Repositories;

namespace MoneyManager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMoneyManagerApplication();
            services.AddInfrastructureRegistrationServices(Configuration);
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddTransient<ApiKeyMiddleware>();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerGen();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MonayManagerAPI"));

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
