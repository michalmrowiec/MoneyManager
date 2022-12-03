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

namespace MoneyManager.API.IntegrationTests.ControllerTests.StatisticsControllerTests
{
    public class GetTotalsForCategoryForEachMonthsOfYearTests : ControllerTestBase
    {
        public GetTotalsForCategoryForEachMonthsOfYearTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_ListOfRecordsWithAssignedCategory => new List<object[]>
        {
            new object[]
            {
                new int[] { 2020, 1 },
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Test1", Amount = 10M, TransactionDate = new DateTime(2020, 01, 01), CategoryId = 1 },
                    new CreateRecordCommand { Id = 2, Name = "Test2", Amount = 1M, TransactionDate = new DateTime(2020, 01, 21), CategoryId = 1 },
                    new CreateRecordCommand { Id = 3, Name = "Test3", Amount = 2M, TransactionDate = new DateTime(2020, 01, 21), CategoryId = 2 },
                    new CreateRecordCommand { Id = 4, Name = "Test4", Amount = 7M, TransactionDate = new DateTime(2020, 02, 12), CategoryId = 1 },
                },
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "Cat1" },
                    new CreateCategoryCommand { Id = 2, Name = "Cat2" }

                },
                new Dictionary<int, decimal> { { 1, 11 }, { 2, 7 }}
            }
        };

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsWithAssignedCategory))]
        public async Task GetRecordsForCategory_ForValidData_ReturnsOkStatusWithListOfRecordsForGivenCategoryId
            (int[] yearAndCategoryId, List<CreateRecordCommand> CreateRecordCommands, List<CreateCategoryCommand> CreateCategoryCommands, Dictionary<int, decimal> result)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, CreateCategoryCommands, "/api/category");
            await TestUtils.PostItemsByListAsync(_httpClient, CreateRecordCommands, "/api/tracker");

            var response = await _httpClient.GetAsync($"/api/stat/totalcatformonths/{yearAndCategoryId[1]}/{yearAndCategoryId[0]}");
            var json = await response.Content.ReadAsStringAsync();
            var listOfRecordsForCategory = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(json);

            listOfRecordsForCategory.Should().BeEquivalentTo(result);
        }
    }
}
