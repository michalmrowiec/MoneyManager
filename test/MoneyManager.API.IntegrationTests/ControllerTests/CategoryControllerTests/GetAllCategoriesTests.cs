using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.CategoryControllerTests
{
    public class GetAllCategoriesTests : ControllerTestBase
    {
        public GetAllCategoriesTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        // HACK: I don't know what it is

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
    }
}
