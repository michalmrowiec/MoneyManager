using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Records;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.TrackerControllerTests
{
    public class GetRecordsForCategoryTests : ControllerTestBase
    {
        public GetRecordsForCategoryTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

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

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsWithAssignedCategory))]
        public async Task GetRecordsForCategory_ForValidData_ReturnsOkStatusWithListOfRecordsForGivenCategoryId
            (List<CreateRecordCommand> CreateRecordCommands, List<CreateCategoryCommand> CreateCategoryCommands)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, CreateCategoryCommands, "/api/category");
            await TestUtils.PostItemsByListAsync(_httpClient, CreateRecordCommands, "/api/tracker");

            var categoryId = CreateCategoryCommands.First().Id;

            var response = await _httpClient.GetAsync($"/api/tracker/cat/{categoryId}");
            var json = await response.Content.ReadAsStringAsync();
            var listOfRecordsForCategory = JsonConvert.DeserializeObject<List<RecordDto>>(json);

            listOfRecordsForCategory.Should().HaveCount(CreateRecordCommands.Where(x => x.CategoryId == categoryId).Count());
        }
    }
}
