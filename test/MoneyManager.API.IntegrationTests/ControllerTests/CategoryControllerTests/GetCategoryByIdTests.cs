using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.CategoryControllerTests
{
    public class GetCategoryByIdTests : ControllerTestBase
    {
        public GetCategoryByIdTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> CategoriesWithValidDataSingleCategory => new List<object[]>
        {
            new object[]
            {
                new CreateCategoryCommand { Name = "Test1" },
                1,
                new CategoryDto { Id = 1, Name = "Test1" }
            },
            new object[]
            {
                new CreateCategoryCommand { Name = "Test1_67" },
                1,
                new CategoryDto { Id = 1, Name = "Test1_67" }
            },
            new object[]
            {
                new CreateCategoryCommand { Id = 9102, Name = "gh% gj   hj" },
                9102,
                new CategoryDto { Id = 9102, Name = "gh% gj   hj" }
            }
        };

        public static IEnumerable<object[]> CategoriesWithValidDataManyCategories => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Name = "Test category0" },
                    new CreateCategoryCommand { Name = "Test category1" },
                    new CreateCategoryCommand { Name = "Test category2" },
                    new CreateCategoryCommand { Name = "Test category3" }
                },
                1,
                new CategoryDto { Id = 1, Name = "Test category0" }
            },
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Name = "Test category0" },
                    new CreateCategoryCommand { Id = 222, Name = "Test category1" },
                    new CreateCategoryCommand { Name = "Test category2" },
                    new CreateCategoryCommand { Name = "Test category3" },
                    new CreateCategoryCommand { Name = "@3" },
                    new CreateCategoryCommand { Name = "  f" },
                    new CreateCategoryCommand { Name = "#@^)(^#gh" }
                },
                223,
                new CategoryDto { Id = 223, Name = "Test category2" }
            },
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 90,Name = " ^" },
                    new CreateCategoryCommand { Id = 222, Name = "1f" },
                    new CreateCategoryCommand { Id = 244,Name = "Test $h34" },
                    new CreateCategoryCommand { Id = 42,Name = "Test #%@#" }
                },
                90,
                new CategoryDto { Id = 90, Name = " ^" }
            }
        };

        public static IEnumerable<object[]> IdWithNoExistCategories => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>{ },
                -1
            },
            new object[]
            {
                new List<CreateCategoryCommand>{ },
                0
            },
            new object[]
            {
                new List<CreateCategoryCommand>{ },
                1
            },
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Name = "Test category0" },
                    new CreateCategoryCommand { Name = "Test category1" },
                    new CreateCategoryCommand { Name = "Test category2" },
                    new CreateCategoryCommand { Name = "Test category3" }
                },
                411
            },
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 123, Name = "Test category0" },
                    new CreateCategoryCommand { Id = 124, Name = "Test category1" }
                },
                125
            },
        };

        [Theory]
        [MemberData(nameof(CategoriesWithValidDataSingleCategory))]
        public async Task GetCategoryById_ForValidDataFromSingleCategories_ReturnsOkStatusWithCategory(CreateCategoryCommand category, int idCategory, CategoryDto expectegCategory)
        {
            await TestUtils.PostItemAsync(_httpClient, category, "api/category");

            var response = await _httpClient.GetAsync($"/api/category/{idCategory}");
            var json = await response.Content.ReadAsStringAsync();
            var categoryDto = JsonConvert.DeserializeObject<CategoryDto>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            categoryDto.Should().BeEquivalentTo(expectegCategory);
        }

        [Theory]
        [MemberData(nameof(CategoriesWithValidDataManyCategories))]
        public async Task GetCategoryById_ForValidDataFromManyCategories_ReturnsOkStatusWithCategory(List<CreateCategoryCommand> categories, int idCategory, CategoryDto expectegCategory)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, categories, "api/category");

            var response = await _httpClient.GetAsync($"/api/category/{idCategory}");
            var json = await response.Content.ReadAsStringAsync();
            var categoryDto = JsonConvert.DeserializeObject<CategoryDto>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            categoryDto.Should().BeEquivalentTo(expectegCategory);
        }

        [Theory]
        [MemberData(nameof(IdWithNoExistCategories))]
        public async Task GetCategoryById_ForNoExistCategory_ReturnsBadRequest(List<CreateCategoryCommand> categories, int categoryId)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, categories, "api/category");

            var response = await _httpClient.GetAsync($"/api/category/{categoryId}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
