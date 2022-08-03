using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
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

        public static IEnumerable<object[]> TestCategories => new List<object[]>
        {
            new object[] { new CreateCategoryCommand { Id = 1, Name = "test" }},
            new object[] { new CreateCategoryCommand { Id = 2, Name = "twenty five character 123" }},
            new object[] { new CreateCategoryCommand { Id = 3, Name = "!@#$%^&*()_+TEST" }},
            new object[] { new CreateCategoryCommand { Id = 4, Name = "TE ST" }},
            new object[] { new CreateCategoryCommand { Id = 5, Name = "90" }}
        };

        [Theory]
        [MemberData(nameof(TestCategories))]
        public async Task CreateCategory_WithValidModel_ReturnsCreatedStatus(CreateCategoryCommand category)
        {
            var json = JsonConvert.SerializeObject(category);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/category", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var createdCategory = JsonConvert.DeserializeObject<CreateCategoryCommand>(jsonResponse);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Theory]
        [MemberData(nameof(TestCategories))]
        public async Task DeleteCatgory_ForExistCateogry_ReturnsNoContentStatus(CreateCategoryCommand category)
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
            var category = new CreateCategoryCommand { Id = 32900, Name = "Test_Category1" };
            var jsonCategory = JsonConvert.SerializeObject(category);
            var httpContent = new StringContent(jsonCategory, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/category", httpContent);

            var response = await _httpClient.GetAsync("/api/category");
            var json = await response.Content.ReadAsStringAsync();
            var listOfCategories = JsonConvert.DeserializeObject<List<CategoryDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            listOfCategories.Should().BeOfType<List<CategoryDto>>().And.HaveCount(1);
        }

        [Fact]
        public async Task UpdateCategory_ForExistRecordAndValidData_ReturnsOkStatus()
        {
            var categoryToCreate = new CreateCategoryCommand { Id = 32900, Name = "Test_Category3200" };
            var jsonCreate = JsonConvert.SerializeObject(categoryToCreate);
            var httpContentCreate = new StringContent(jsonCreate, UnicodeEncoding.UTF8, "application/json");

            var categoryToUpdate = new CreateCategoryCommand { Id = 32900, Name = "Test_CategoryUpdated" };
            var jsonUpdate = JsonConvert.SerializeObject(categoryToUpdate);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");

            await _httpClient.PostAsync("/api/category", httpContentCreate);
            var response = await _httpClient.PutAsync("/api/category", httpContentUpdate);

            var responseGetAllCategory = await _httpClient.GetAsync("/api/category");
            var jsonResponseGetAllCategories = await responseGetAllCategory.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(jsonResponseGetAllCategories);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            categories.Should().BeEquivalentTo(new List<CategoryDto> { new CategoryDto { Id = 32900, Name = "Test_CategoryUpdated" } });
        }
    }
}