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
    public class UpdateCategoryTests : ControllerTestBase
    {
        public UpdateCategoryTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

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
