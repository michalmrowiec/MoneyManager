using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Domain.Entities;
using MoneyManager.Infractructure.Authentication;
using MoneyManager.Infractructure.Repositories.Items;
using MoneyManager.Infractructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure
{
    public static class EFRegistration
    {
        public static IServiceCollection AddEFRegistrationServices(this IServiceCollection services, IConfiguration configuration)
        {

            var authenticationSettings = new AuthenticationSettings();
            configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            services.AddSingleton(authenticationSettings);

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddDbContext<MoneyManagerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TrackerDbConnectionPc")));

            services.AddScoped(typeof(IItemAsyncRepositoryBase<>), typeof(ItemRepositoryBase<>));
            services.AddScoped(typeof(IUserAsyncRepository), typeof(UserRepository));

            services.AddScoped<IRecordRepsitory, RecordRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
