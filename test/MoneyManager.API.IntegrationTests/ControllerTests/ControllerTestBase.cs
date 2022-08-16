﻿using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Infractructure;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests
{
    public class ControllerTestBase : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _httpClient;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public ControllerTestBase(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var ef = services.SingleOrDefault(services => services.ServiceType == typeof(EFRegistration));
                    if (ef != null) services.Remove(ef);

                    var db = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<MoneyManagerContext>));
                    if (db != null) services.Remove(db);

                    // Thanks this line, while processed query which require authentication (for endpoints with atrubute [Authorize])
                    // Execute evaluation will be delegate for FakePolicyEvaluatro class
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    // Register user filter
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    // Add fake memory db
                    services.AddDbContext<MoneyManagerContext>(options => options.UseInMemoryDatabase(_dbName));
                });
            }).CreateClient();
        }
    }
}