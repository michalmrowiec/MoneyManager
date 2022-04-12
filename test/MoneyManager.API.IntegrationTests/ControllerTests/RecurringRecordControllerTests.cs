using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
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
    public class RecurringRecordControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public RecurringRecordControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var ef = services.SingleOrDefault(services => services.ServiceType == typeof(EFRegistration));
                    if (ef != null) services.Remove(ef);

                    var db = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<MoneyManagerContext>));
                    if (db != null) services.Remove(db);

                    // thanks this line, while processed query which require authentication (for endpoints with atrubute [Authorize])
                    // execute evaluation will be delegate for FakePolicyEvaluatro class
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    // register user filter
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    // add fake memory db
                    services.AddDbContext<MoneyManagerContext>(options => options.UseInMemoryDatabase(_dbName));
                });
            })
            .CreateClient();
        }

        public static IEnumerable<object[]> Test_CreateRecurringRecord => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "Test Category" },
                },
                new List<CreateRecurringRecordCommand>
                {
                    new CreateRecurringRecordCommand
                    { Id = 1, IsActive = true, NextDate = new DateTime(2022, 02, 01), RepeatEveryDayOfMonth = 1,Name = "Test", Amount = 100M, TransactionDate = new DateTime(2022, 01, 01), CategoryId = 1, UserId = 1 },
                },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 01, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                    new RecordDto { Id = 2, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 02, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                    new RecordDto { Id = 3, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 03, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                    new RecordDto { Id = 4, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 04, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_CreateRecurringRecord))]
        public async Task CreateRecurringRecord_WithValidModel_ReturnsCreatedStatus
            (List<CreateCategoryCommand> createCategories, List<CreateRecurringRecordCommand> createRecurrings, List<RecordDto> resultRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, createCategories, "/api/category");
            await TestUtils.PostRecordsByList(_httpClient, createRecurrings, "api/recurring");

            var response = await _httpClient.GetAsync("api/recurring/ex/20220401");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(Test_CreateRecurringRecord))]
        public async Task ExecuteRecurringRecords_ForExistRecords_ShouldCreateRecords
            (List<CreateCategoryCommand> createCategories, List<CreateRecurringRecordCommand> createRecurrings, List<RecordDto> resultRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, createCategories, "/api/category");
            await TestUtils.PostRecordsByList(_httpClient, createRecurrings, "api/recurring");

            var response = await _httpClient.GetAsync("api/recurring/ex/20220401");

            var response2 = await _httpClient.GetAsync("/api/tracker");
            var json = await response2.Content.ReadAsStringAsync();
            var returnedListOfRecords = JsonConvert.DeserializeObject<List<RecordDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            returnedListOfRecords.Should().BeEquivalentTo(resultRecords);
        }
    }
}
