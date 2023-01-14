using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Records;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.TrackerControllerTests
{
    public class GetAllRecordsTests : ControllerTestBase
    {
        public GetAllRecordsTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_ListOfRecords => new List<object[]>
        {
            new object[]
            {
                new CreateCategoryCommand { Id = 1, Name = "Test1" },
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01), CategoryId = 1 },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11), CategoryId = 1 },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30), CategoryId = 1 },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M, TransactionDate = new DateTime(2012, 03, 01), CategoryId = 1 },
                    new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 1 }
                }
            },
            new object[]
            {
                new CreateCategoryCommand { Id = 22, Name = "Test2" },
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 9800, Name = "*LEsadf)(*", Amount = 79228162514.26M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 22 },
                    new CreateRecordCommand { Id = 34779, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79.09M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 22 },
                    new CreateRecordCommand { Id = 965443, Name = "TestR", Amount = 0M, TransactionDate = new DateTime(2020, 10, 30), CategoryId = 22 }
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_ListOfRecords))]
        public async Task GetAllRecords_ForNoData_ReturnsOkStatusWithListOfRecords
            (CreateCategoryCommand createCategoryCommand, List<CreateRecordCommand> listOfRecords)
        {
            await TestUtils.PostItemAsync(_httpClient, createCategoryCommand, "/api/category");
            await TestUtils.PostItemsByListAsync(_httpClient, listOfRecords, "/api/tracker");

            var response = await _httpClient.GetAsync("/api/tracker");
            var json = await response.Content.ReadAsStringAsync();
            var returnedListOfRecords = JsonConvert.DeserializeObject<List<RecordDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            returnedListOfRecords.Should().BeOfType<List<RecordDto>>().And.HaveCount(listOfRecords.Count);//.And.BeEquivalentTo(listOfRecords);
        }
    }
}
