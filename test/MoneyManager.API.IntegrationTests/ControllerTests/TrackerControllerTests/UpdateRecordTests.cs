using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.TrackerControllerTests
{
    public class UpdateRecordTests : ControllerTestBase
    {
        public UpdateRecordTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_ListOfRecordsToUpdate => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "test" }
                },
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01), CategoryId = 1 },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11), CategoryId = 1 },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30), CategoryId = 1 },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M, CategoryId = 1 },
                    new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 1 }
                },
                new UpdateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, TransactionDate = new DateTime(2013, 08, 11), CategoryId = 1, UserId = 1 },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01), CategoryId = 1, Category = new Category {  Id = 1, Name = "test", UserId = 1 } },
                    new RecordDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, TransactionDate = new DateTime(2013, 08, 11), CategoryId = 1, Category = new Category { Id = 1, Name = "test", UserId = 1 } },
                    new RecordDto { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30), CategoryId = 1, Category = new Category { Id = 1, Name = "test", UserId = 1 } },
                    new RecordDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M, CategoryId = 1, Category = new Category { Id = 1, Name = "test", UserId = 1 } },
                    new RecordDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 1, Category = new Category { Id = 1, Name = "test", UserId = 1 } }
                }
            },
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 2, Name = "test2" }
                },
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01), CategoryId = 2 },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11), CategoryId = 2 },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30), CategoryId = 2 },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M, CategoryId = 2 },
                    new CreateRecordCommand { Id = 6, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2 },
                    new CreateRecordCommand { Id = 779, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2 },
                    new CreateRecordCommand { Id = 88, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2 }
                },
                new UpdateRecordCommand { Id = 779, Name = "TEST", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2, UserId = 1 },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01), CategoryId = 2, Category = new Category {  Id = 2, Name = "test2", UserId = 1 } },
                    new RecordDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11), CategoryId = 2, Category = new Category { Id = 2, Name = "test2", UserId = 1 } },
                    new RecordDto { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30), CategoryId = 2, Category = new Category { Id = 2, Name = "test2", UserId = 1 } },
                    new RecordDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M, CategoryId = 2, Category = new Category { Id = 2, Name = "test2", UserId = 1 } },
                    new RecordDto { Id = 6, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2, Category = new Category { Id = 2, Name = "test2", UserId = 1 } },
                    new RecordDto { Id = 779, Name = "TEST", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2, Category = new Category { Id = 2, Name = "test2", UserId = 1 } },
                    new RecordDto { Id = 88, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), CategoryId = 2, Category = new Category { Id = 2, Name = "test2", UserId = 1 } }
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsToUpdate))]
        public async Task UpdateRecord_ForExistRecordAndValidData_ReturnsOkStatus
            (List<CreateCategoryCommand> createCategoryCommands, List<CreateRecordCommand> createRecordCommands,
                UpdateRecordCommand recordToUpdate, List<RecordDto> updatedRecords)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, createCategoryCommands, "/api/category");
            await TestUtils.PostItemsByListAsync(_httpClient, createRecordCommands, "/api/tracker");

            var jsonUpdate = JsonConvert.SerializeObject(recordToUpdate);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/tracker", httpContentUpdate);

            var responseGetAllRecords = await _httpClient.GetAsync("/api/tracker");
            var jsonResponseGetAllRecords = await responseGetAllRecords.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<List<RecordDto>>(jsonResponseGetAllRecords);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            records.Should().BeEquivalentTo(updatedRecords);
        }
    }
}
