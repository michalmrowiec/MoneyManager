using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Records;
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
    public class TrackerControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public TrackerControllerTests(WebApplicationFactory<Startup> factory)
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

        public static IEnumerable<object[]> Test_SingleRecords => new List<object[]>
        {
            new object[] { new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) } },
            new object[] { new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) } },
            new object[] { new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) } },
            new object[] { new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M } },
            new object[] { new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) } }
        };

        public static IEnumerable<object[]> Test_ListOfRecords => new List<object[]>
        {
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                }
            },
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 9800, Name = "*LEsadf)(*", Amount = 79228162514.26M, TransactionDate = new DateTime(2020, 10, 30) },
                    new CreateRecordCommand { Id = 34779, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79.09M, TransactionDate = new DateTime(2020, 10, 30) },
                    new CreateRecordCommand { Id = 965443, Name = "TestR", Amount = 0M, TransactionDate = new DateTime(2020, 10, 30) }
                }
            }
        };

        public static IEnumerable<object[]> Test_ListOfRecordsWithAssignedCategory => new List<object[]>
        {
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                },
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 324, Name = "Test1" },
                    new CreateCategoryCommand { Id = 479476, Name = "!@#$%" },
                    new CreateCategoryCommand { Id = 347, Name = "23" },
                    new CreateCategoryCommand { Id = 8654, Name = "SDg DFG  554%" }
                }
            },
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 9800, Name = "Test1", Amount = 79228162514.26M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 13954, CategoryName = "Retas" },
                    new CreateRecordCommand { Id = 34779, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79.09M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 13954, CategoryName = "Retas" },
                    new CreateRecordCommand { Id = 965443, Name = "TestR", Amount = 0M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 68, CategoryName = "S P A C E" }
                },
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 13954, Name = "Retas" },
                    new CreateCategoryCommand { Id = 5694, Name = "TEST_23" },
                    new CreateCategoryCommand { Id = 68, Name = "S P A C E" }
                }
            },
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 568, Name = "54376gfdh", Amount = -74.26M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 1124, CategoryName = "S P A C E" },
                    new CreateRecordCommand { Id = 4569, Name = "test", Amount = -69.65M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 1124, CategoryName = "S P A C E" },
                    new CreateRecordCommand { Id = 5678, Name = "opu{}", Amount = 678.87M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 1124, CategoryName = "S P A C E" }
                },
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 88, Name = "Retas" },
                    new CreateCategoryCommand { Id = 654, Name = "TEST_23" },
                    new CreateCategoryCommand { Id = 1124, Name = "S P A C E" }
                }
            }
        };

        public static IEnumerable<object[]> Test_SingleCategories => new List<object[]>
        {
            new object[] { new CreateCategoryCommand { Id = 324, Name = "Test1" } },
            new object[] { new CreateCategoryCommand { Id = 479476, Name = "!@#$%" } },
            new object[] { new CreateCategoryCommand { Id = 347, Name = "23" } },
            new object[] { new CreateCategoryCommand { Id = 8654, Name = "SDg DFG  554%" } },

        };

        public static IEnumerable<object[]> Test_ListOfRecordsToUpdate => new List<object[]>
        {
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                },
                new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, TransactionDate = new DateTime(2013, 08, 11) },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, TransactionDate = new DateTime(2013, 08, 11) },
                    new RecordDto { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new RecordDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                }
            },
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new CreateRecordCommand { Id = 6, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new CreateRecordCommand { Id = 779, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new CreateRecordCommand { Id = 88, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                },
                new CreateRecordCommand { Id = 779, Name = "TEST", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new RecordDto { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new RecordDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordDto { Id = 6, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new RecordDto { Id = 779, Name = "TEST", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new RecordDto { Id = 88, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_SingleRecords))]
        public async Task CreateRecord_WithValidModel_ReturnsCreatedStatus(CreateRecordCommand record)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/tracker", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Theory]
        [MemberData(nameof(Test_SingleRecords))]
        public async Task DeleteRecord_ForExistRecord_ReturnsNoContentStatus(CreateRecordCommand record)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/tracker", httpContent);

            var response = await _httpClient.DeleteAsync($"/api/tracker/{record.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [MemberData(nameof(Test_ListOfRecords))]
        public async Task GetAllRecords_ForNoData_ReturnsOkStatusWithListOfRecords(List<CreateRecordCommand> listOfRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, listOfRecords, "/api/tracker");

            var response = await _httpClient.GetAsync("/api/tracker");
            var json = await response.Content.ReadAsStringAsync();
            var returnedListOfRecords = JsonConvert.DeserializeObject<List<RecordDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            returnedListOfRecords.Should().BeOfType<List<RecordDto>>().And.HaveCount(listOfRecords.Count);//.And.BeEquivalentTo(listOfRecords);
        }

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsWithAssignedCategory))]
        public async Task GetRecordsForCategory_ForValidData_ReturnsOkStatusWithListOfRecordsForGivenCategoryId
            (List<CreateRecordCommand> CreateRecordCommands, List<CreateCategoryCommand> CreateCategoryCommands)
        {
            await TestUtils.PostRecordsByList(_httpClient, CreateRecordCommands, "/api/tracker");
            await TestUtils.PostRecordsByList(_httpClient, CreateCategoryCommands, "/api/category");

            var categoryId = CreateCategoryCommands.First().Id;

            var response = await _httpClient.GetAsync($"/api/tracker/cat/{categoryId}");
            var json = await response.Content.ReadAsStringAsync();
            var listOfRecordsForCategory = JsonConvert.DeserializeObject<List<RecordDto>>(json);

            listOfRecordsForCategory.Should().HaveCount(CreateRecordCommands.Where(x => x.CategoryId == categoryId).Count());
        }

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsToUpdate))]
        public async Task UpdateRecord_ForExistRecordAndValidData_ReturnsOkStatus
            (List<CreateRecordCommand> CreateRecordCommands, CreateRecordCommand recordToUpdate, List<RecordDto> updatedRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, CreateRecordCommands, "/api/tracker");

            var jsonUpdate = JsonConvert.SerializeObject(recordToUpdate);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync("/api/tracker", httpContentUpdate);

            var responseGetAllRecords = await _httpClient.GetAsync("/api/tracker");
            var jsonResponseGetAllRecords = await responseGetAllRecords.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<List<RecordDto>>(jsonResponseGetAllRecords);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            records.Should().BeEquivalentTo(updatedRecords);
        }
    }
}