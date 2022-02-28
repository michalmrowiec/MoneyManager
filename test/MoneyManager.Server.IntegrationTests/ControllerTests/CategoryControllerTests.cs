using MoneyManager.Server.Entities;
using MoneyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.Server.IntegrationTests.ControllerTests
{
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public CategoryControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                    .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<TrackerDbContext>));

                    if (dbContextOptions != null) services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    services.AddDbContext<TrackerDbContext>(options => options.UseInMemoryDatabase(_dbName));
                });
            })
                .CreateClient();
        }

        public static IEnumerable<object[]> TestCategories => new List<object[]>
        {
            new object[] { new CategoryItemDto { Id = 1, Name = "test" }},
            new object[] { new CategoryItemDto { Id = 2, Name = "twenty five character 123" }},
            new object[] { new CategoryItemDto { Id = 3, Name = "!@#$%^&*()_+TEST" }},
            new object[] { new CategoryItemDto { Id = 4, Name = "TE ST" }},
            new object[] { new CategoryItemDto { Id = 5, Name = "90" }}
        };

        [Theory]
        [MemberData(nameof(TestCategories))]
        public async Task CreateCategory_WithValidModel_ReturnsCreatedStatusAndCreatedCategory(CategoryItemDto category)
        {
            var json = JsonConvert.SerializeObject(category);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/category", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var createdCategory = JsonConvert.DeserializeObject<CategoryItemDto>(jsonResponse);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            createdCategory.Should().BeEquivalentTo(category);
        }

        [Theory]
        [MemberData(nameof(TestCategories))]
        public async Task DeleteCatgory_ForExistCateogry_ReturnsNoContentStatus(CategoryItemDto category)
        {
            var json = JsonConvert.SerializeObject(category);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/category", httpContent);

            var response = await _httpClient.DeleteAsync($"/api/category/{category.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetAllCategories_ForNoData_ReturnsOkStatusWithListOfCategories()
        {
            var category = new CategoryItemDto { Id = 32900, Name = "Test_Category1" };
            var jsonCategory = JsonConvert.SerializeObject(category);
            var httpContent = new StringContent(jsonCategory, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/category", httpContent);

            var response = await _httpClient.GetAsync("/api/category");
            var json = await response.Content.ReadAsStringAsync();
            var listOfCategories = JsonConvert.DeserializeObject<List<CategoryItemDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            listOfCategories.Should().BeOfType<List<CategoryItemDto>>().And.HaveCount(1);
        }

        [Fact]
        public async Task UpdateCategory_ForExistRecordAndValidData_ReturnsOkStatus()
        {
            var categoryToCreate = new CategoryItemDto { Id = 32900, Name = "Test_Category3200" };
            var jsonCreate = JsonConvert.SerializeObject(categoryToCreate);
            var httpContentCreate = new StringContent(jsonCreate, UnicodeEncoding.UTF8, "application/json");

            var categoryToUpdate = new CategoryItemDto { Id = 32900, Name = "Test_CategoryUpdated" };
            var jsonUpdate = JsonConvert.SerializeObject(categoryToUpdate);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/category", httpContentCreate);
            var response = await _httpClient.PatchAsync("/api/category", httpContentUpdate);

            var responseGetAllCategory = await _httpClient.GetAsync("/api/category");
            var jsonResponseGetAllCategories = await responseGetAllCategory.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryItemDto>>(jsonResponseGetAllCategories);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            categories.Should().HaveCount(1).And.ContainEquivalentOf(categoryToUpdate);
        }
    }
}
