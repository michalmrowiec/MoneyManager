using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MoneyManager.Application
{
    public static class ApplicationInstallation
    {
        public static IServiceCollection AddMoneyManagerApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());


            services.AddMediatR(typeof(ApplicationInstallation));

            return services;
        }
    }
}
