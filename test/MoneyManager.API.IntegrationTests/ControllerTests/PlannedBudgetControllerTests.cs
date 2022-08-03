using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Commands.UpdatePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using MoneyManager.Infractructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests
{
    public class PlannedBudgetControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public PlannedBudgetControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var ef = services.SingleOrDefault(services => services.ServiceType == typeof(EFRegistration));
                    if (ef != null) services.Remove(ef);

                    var db = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<MoneyManagerContext>));
                    if (db != null) services.Remove(db);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    services.AddDbContext<MoneyManagerContext>(options => options.UseInMemoryDatabase(_dbName));
                });
            }).CreateClient();
        }

        public static IEnumerable<object[]> Test_SinglePlannedBudget => new List<object[]>
        {
            new object[] { new CreatePlannedBudgetCommand { Id = 1, Amount = 100M, PlanForMonth = new DateTime(2010, 09, 01) } },
            new object[] { new CreatePlannedBudgetCommand { Id = 2, Amount = 3213M, PlanForMonth = new DateTime(2018, 06, 01) } },
            new object[] { new CreatePlannedBudgetCommand { Id = 3, Amount = 89.23M, PlanForMonth = new DateTime(2022, 11, 01) } }
        };

        public static IEnumerable<object[]> Test_UpdatePlannedBudget => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "Test1" },
                    new CreateCategoryCommand { Id = 2, Name = "!@#$%" },
                    new CreateCategoryCommand { Id = 3, Name = "23" },
                    new CreateCategoryCommand { Id = 4, Name = "SDg DFG  554%" }
                },
                new List<CreatePlannedBudgetCommand>
                {
                    new CreatePlannedBudgetCommand { Id = 1, Amount = 1040M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01) },
                    new CreatePlannedBudgetCommand { Id = 2, Amount = 3M, CategoryId = 4, PlanForMonth = new DateTime(2012, 06, 01) },
                    new CreatePlannedBudgetCommand { Id = 3, Amount = 89.23M, CategoryId = 2, PlanForMonth = new DateTime(2021, 12, 01) }
                },
                new UpdatePlannedBudgetCommand { Id = 3, Amount = 89.29M, CategoryId = 3, PlanForMonth = new DateTime(2021, 12, 01) },
                new List<PlannedBudgetDto>
                {
                    new PlannedBudgetDto { Id = 1, Amount = 1040M, FilledAmount = 0M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01), UserId = 1 },
                    new PlannedBudgetDto { Id = 2, Amount = 3M, FilledAmount = 0M, CategoryId = 4, PlanForMonth = new DateTime(2012, 06, 01), UserId = 1 },
                    new PlannedBudgetDto { Id = 3, Amount = 89.29M, FilledAmount = 0M, CategoryId = 3, PlanForMonth = new DateTime(2021, 12, 01), UserId = 1 }
                }
            }
        };

        public static IEnumerable<object[]> Test_GetPlannedBudgets => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "Test1" },
                    new CreateCategoryCommand { Id = 2, Name = "!@#$%" },
                    new CreateCategoryCommand { Id = 3, Name = "23" },
                    new CreateCategoryCommand { Id = 4, Name = "SDg DFG  554%" }
                },
                new List<CreatePlannedBudgetCommand>
                {
                    new CreatePlannedBudgetCommand { Id = 1, Amount = 1040M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01) },
                    new CreatePlannedBudgetCommand { Id = 2, Amount = 3M, CategoryId = 4, PlanForMonth = new DateTime(2012, 06, 01) },
                    new CreatePlannedBudgetCommand { Id = 3, Amount = 89.23M, CategoryId = 2, PlanForMonth = new DateTime(2021, 12, 01) },
                    new CreatePlannedBudgetCommand { Id = 4, Amount = 6222M, CategoryId = 2, PlanForMonth = new DateTime(2001, 04, 01) }
                },
                new int[] { 2001 },
                new int[] { 4 },
                new List<PlannedBudgetDto>
                {
                    new PlannedBudgetDto { Id = 1, Amount = 1040M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01), UserId = 1 },
                    new PlannedBudgetDto { Id = 4, Amount = 6222M, CategoryId = 2, PlanForMonth = new DateTime(2001, 04, 01), UserId = 1 },
                },
            }
        };

        [Theory]
        [MemberData(nameof(Test_SinglePlannedBudget))]
        public async Task CreatePlannedBudget_WithValidModel_ReturnsCreatedStatus(CreatePlannedBudgetCommand createPlannedBudget)
        {
            var json = JsonConvert.SerializeObject(createPlannedBudget);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/plannedbudget", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Theory]
        [MemberData(nameof(Test_SinglePlannedBudget))]
        public async Task DeletePlannedBudget_ForExistPlannedBudgets_ReturnsNoContentStatus(CreatePlannedBudgetCommand plannedBudget)
        {
            var json = JsonConvert.SerializeObject(plannedBudget);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/plannedbudget", httpContent);

            var response = await _httpClient.DeleteAsync($"/api/plannedbudget/{plannedBudget.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [MemberData(nameof(Test_UpdatePlannedBudget))]
        public async Task UpdatePlannedBudget_ForExistRexordsAndValidData_ReturnsOkStatus
            (List<CreateCategoryCommand> createCategories, List<CreatePlannedBudgetCommand> createPlannedBudgets,
            UpdatePlannedBudgetCommand updatePlannedBudget, List<PlannedBudgetDto> updatedPlannedBudgets)
        {
            await TestUtils.PostRecordsByList(_httpClient, createCategories, "/api/category");
            await TestUtils.PostRecordsByList(_httpClient, createPlannedBudgets, "/api/plannedbudget");

            var jsonUpdate = JsonConvert.SerializeObject(updatePlannedBudget);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/plannedbudget", httpContentUpdate);

            var responseGetAllPlannedBudgets = await _httpClient.GetAsync("/api/plannedbudget");
            var jsonResponseGetAllPlannedBudgets = await responseGetAllPlannedBudgets.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<List<PlannedBudgetDto>>(jsonResponseGetAllPlannedBudgets);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            records.Should().BeEquivalentTo(updatedPlannedBudgets);
        }

        [Theory]
        [MemberData(nameof(Test_GetPlannedBudgets))]
        public async Task GetPlannedBudgetsForMonth_ForValidData_ReturnsListOfPlannedBudgetsForGivenMonth
            (List<CreateCategoryCommand> createCategories, List<CreatePlannedBudgetCommand> createPlannedBudgets,
            int[] year, int[] month, List<PlannedBudgetDto> expectedResult)
        {
            await TestUtils.PostRecordsByList(_httpClient, createCategories, "/api/category");
            await TestUtils.PostRecordsByList(_httpClient, createPlannedBudgets, "/api/plannedbudget");

            var responseMessage = await _httpClient.GetAsync($"/api/plannedbudget/{year[0]}/{month[0]}");
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            var plannedBudgetsDtos = JsonConvert.DeserializeObject<List<PlannedBudgetDto>>(jsonResponse);

            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            plannedBudgetsDtos.Should().BeEquivalentTo(expectedResult);
        }
    }
}
