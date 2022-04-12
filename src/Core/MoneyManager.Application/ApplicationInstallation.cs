using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
