using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.CategoryControllerTests
{
    public class CreateCategoryTests : ControllerTestBase
    {
        public CreateCategoryTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

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

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }
    }
}
