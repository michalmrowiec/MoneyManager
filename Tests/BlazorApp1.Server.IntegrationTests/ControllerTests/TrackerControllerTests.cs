using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorApp1.Shared;
using Newtonsoft.Json;
using System.Text;
using FluentAssertions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.Server.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Collections.Generic;
using System;
using BlazorApp1.Server.IntegrationTests.ControllerTests.ControllerTestUtils;

namespace BlazorApp1.Server.IntegrationTests.ControllerTests
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
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<TrackerDbContext>));

                    if (dbContextOptions != null) services.Remove(dbContextOptions);

                    // thanks this line, while processed query which require authentication (for endpoints with atrubute [Authorize])
                    // execute evaluation will be delegate for FakePolicyEvaluatro class
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    // register user filter
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    // add fake memory db
                    services.AddDbContext<TrackerDbContext>(options => options.UseInMemoryDatabase(_dbName));
                });
            })
            .CreateClient();
        }

        public static IEnumerable<object[]> Test_SingleRecords => new List<object[]>
        {
            new object[] { new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) } },
            new object[] { new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, Date = new DateTime(2013, 07, 11) } },
            new object[] { new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) } },
            new object[] { new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M } },
            new object[] { new RecordItemDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) } }
        };

        public static IEnumerable<object[]> Test_ListOfRecords => new List<object[]>
        {
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, Date = new DateTime(2013, 07, 11) },
                    new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) },
                    new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordItemDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) }
                }
            },
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 9800, Name = "*LEsadf)(*", Amount = 79228162514.26M, Date = new DateTime(2020, 10, 30) },
                    new RecordItemDto { Id = 34779, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79.09M, Date = new DateTime(2020, 10, 30) },
                    new RecordItemDto { Id = 965443, Name = "TestR", Amount = 0M, Date = new DateTime(2020, 10, 30) }
                }
            }
        };

        public static IEnumerable<object[]> Test_ListOfRecordsWithAssignedCategory => new List<object[]>
        {
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, Date = new DateTime(2013, 07, 11) },
                    new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) },
                    new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordItemDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) }
                },
                new List<CategoryItemDto>
                {
                    new CategoryItemDto { Id = 324, Name = "Test1" },
                    new CategoryItemDto { Id = 479476, Name = "!@#$%" },
                    new CategoryItemDto { Id = 347, Name = "23" },
                    new CategoryItemDto { Id = 8654, Name = "SDg DFG  554%" }
                }
            },
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 9800, Name = "Test1", Amount = 79228162514.26M, Date = new DateTime(2020, 10, 30), CategoryId = 13954, CategoryName = "Retas" },
                    new RecordItemDto { Id = 34779, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79.09M, Date = new DateTime(2020, 10, 30), CategoryId = 13954, CategoryName = "Retas" },
                    new RecordItemDto { Id = 965443, Name = "TestR", Amount = 0M, Date = new DateTime(2020, 10, 30), CategoryId = 68, CategoryName = "S P A C E" }
                },
                new List<CategoryItemDto>
                {
                    new CategoryItemDto { Id = 13954, Name = "Retas" },
                    new CategoryItemDto { Id = 5694, Name = "TEST_23" },
                    new CategoryItemDto { Id = 68, Name = "S P A C E" }
                }
            },
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 568, Name = "54376gfdh", Amount = -74.26M, Date = new DateTime(2020, 10, 30), CategoryId = 1124, CategoryName = "S P A C E" },
                    new RecordItemDto { Id = 4569, Name = "test", Amount = -69.65M, Date = new DateTime(2020, 10, 30), CategoryId = 1124, CategoryName = "S P A C E" },
                    new RecordItemDto { Id = 5678, Name = "opu{}", Amount = 678.87M, Date = new DateTime(2020, 10, 30), CategoryId = 1124, CategoryName = "S P A C E" }
                },
                new List<CategoryItemDto>
                {
                    new CategoryItemDto { Id = 88, Name = "Retas" },
                    new CategoryItemDto { Id = 654, Name = "TEST_23" },
                    new CategoryItemDto { Id = 1124, Name = "S P A C E" }
                }
            }
        };

        public static IEnumerable<object[]> Test_SingleCategories => new List<object[]>
        {
            new object[] { new CategoryItemDto { Id = 324, Name = "Test1" } },
            new object[] { new CategoryItemDto { Id = 479476, Name = "!@#$%" } },
            new object[] { new CategoryItemDto { Id = 347, Name = "23" } },
            new object[] { new CategoryItemDto { Id = 8654, Name = "SDg DFG  554%" } },

        };

        public static IEnumerable<object[]> Test_ListOfRecordsToUpdate => new List<object[]>
        {
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, Date = new DateTime(2013, 07, 11) },
                    new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) },
                    new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordItemDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) }
                },
                new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, Date = new DateTime(2013, 08, 11) },
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, Date = new DateTime(2013, 08, 11) },
                    new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) },
                    new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordItemDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) }
                }
            },
            new object[]
            {
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, Date = new DateTime(2013, 07, 11) },
                    new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) },
                    new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordItemDto { Id = 6, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) },
                    new RecordItemDto { Id = 779, Name = "Test", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) },
                    new RecordItemDto { Id = 88, Name = "Test", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) }
                },
                new RecordItemDto { Id = 779, Name = "TEST", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) },
                new List<RecordItemDto>
                {
                    new RecordItemDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, Date = new DateTime(2020, 01, 01) },
                    new RecordItemDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, Date = new DateTime(2013, 07, 11) },
                    new RecordItemDto { Id = 3, Name = "0000000", Amount = 0M, Date = new DateTime(1999, 12, 30) },
                    new RecordItemDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordItemDto { Id = 6, Name = "Test test test123", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) },
                    new RecordItemDto { Id = 779, Name = "TEST", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) },
                    new RecordItemDto { Id = 88, Name = "Test", Amount = 8965.18M, Date = new DateTime(2019, 05, 04) }
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_SingleRecords))]
        public async Task CreateRecord_WithValidModel_ReturnsCreatedStatus(RecordItemDto record)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/tracker", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Theory]
        [MemberData(nameof(Test_SingleRecords))]
        public async Task DeleteRecord_ForExistRecord_ReturnsNoContentStatus(RecordItemDto record)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/tracker", httpContent);

            var response = await _httpClient.DeleteAsync($"/api/tracker/{record.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [MemberData(nameof(Test_ListOfRecords))]
        public async Task GetAllRecords_ForNoData_ReturnsOkStatusWithListOfRecords(List<RecordItemDto> listOfRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, listOfRecords, "/api/tracker");

            var response = await _httpClient.GetAsync("/api/tracker");
            var json = await response.Content.ReadAsStringAsync();
            var returnedListOfRecords = JsonConvert.DeserializeObject<List<RecordItemDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            returnedListOfRecords.Should().BeOfType<List<RecordItemDto>>().And.HaveCount(listOfRecords.Count).And.BeEquivalentTo(listOfRecords);
        }

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsWithAssignedCategory))]
        public async Task GetRecordsForCategory_ForValidData_ReturnsOkStatusWithListOfRecordsForGivenCategoryId(List<RecordItemDto> recordItemDtos, List<CategoryItemDto> categoryItemDtos)
        {
            await TestUtils.PostRecordsByList(_httpClient, recordItemDtos, "/api/tracker");
            await TestUtils.PostRecordsByList(_httpClient, categoryItemDtos, "/api/category");

            var categoryId = categoryItemDtos.First().Id;

            var response = await _httpClient.GetAsync($"/api/tracker/cat/{categoryId}");
            var json = await response.Content.ReadAsStringAsync();
            var listOfRecordsForCategory = JsonConvert.DeserializeObject<List<RecordItemDto>>(json);

            listOfRecordsForCategory.Should().HaveCount(recordItemDtos.Where(x => x.CategoryId == categoryId).Count());
        }

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsToUpdate))]
        public async Task UpdateRecord_ForExistRecordAndValidData_ReturnsOkStatus(List<RecordItemDto> recordItemDtos, RecordItemDto recordToUpdate, List<RecordItemDto> updatedRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, recordItemDtos, "/api/tracker");

            var jsonUpdate = JsonConvert.SerializeObject(recordToUpdate);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync("/api/tracker", httpContentUpdate);

            var responseGetAllRecords = await _httpClient.GetAsync("/api/tracker");
            var jsonResponseGetAllRecords = await responseGetAllRecords.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<List<RecordItemDto>>(jsonResponseGetAllRecords);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            records.Should().BeEquivalentTo(updatedRecords);
        }

    }
}
