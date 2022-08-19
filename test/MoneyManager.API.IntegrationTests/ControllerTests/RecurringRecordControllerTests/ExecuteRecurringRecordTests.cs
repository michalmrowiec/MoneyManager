using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.RecurringRecordControllerTests
{
    public class ExecuteRecurringRecordTests : ControllerTestBase
    {
        public ExecuteRecurringRecordTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_ExecuteRecurringRecord => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "Test Category" },
                },
                new List<CreateRecurringRecordCommand>
                {
                    new CreateRecurringRecordCommand
                    { Id = 1, IsActive = true, NextDate = new DateTime(2022, 02, 01), RepeatEveryDayOfMonth = 1, Name = "Test", Amount = 100M, TransactionDate = new DateTime(2022, 01, 01), CategoryId = 1, UserId = 1 },
                },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 01, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                    new RecordDto { Id = 2, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 02, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                    new RecordDto { Id = 3, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 03, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                    new RecordDto { Id = 4, Name = "Test", CategoryName = "Test Category", Amount = 100, TransactionDate = new DateTime(2022, 04, 01), CategoryId = 1, Category = new Domain.Entities.Category { Id = 1, CategoryName = "Test Category", UserId = 1 } },
                },
                new string("20220401")
            },
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 2, Name = "Test Category^2" },
                },
                new List<CreateRecurringRecordCommand>
                {
                    new CreateRecurringRecordCommand
                    { IsActive = true, NextDate = new DateTime(2022, 07, 07), RepeatEveryDayOfMonth = 1, Name = "Test2", Amount = 715.78M, TransactionDate = new DateTime(2022, 06, 07), CategoryId = 2, UserId = 1 },
                },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Test2", CategoryName = "Test Category^2", Amount = 715.78M, TransactionDate = new DateTime(2022, 06, 07), CategoryId = 2, Category = new Domain.Entities.Category { Id = 2, CategoryName = "Test Category^2", UserId = 1 } },
                    new RecordDto { Id = 2, Name = "Test2", CategoryName = "Test Category^2", Amount = 715.78M, TransactionDate = new DateTime(2022, 07, 07), CategoryId = 2, Category = new Domain.Entities.Category { Id = 2, CategoryName = "Test Category^2", UserId = 1 } },
                    new RecordDto { Id = 3, Name = "Test2", CategoryName = "Test Category^2", Amount = 715.78M, TransactionDate = new DateTime(2022, 08, 07), CategoryId = 2, Category = new Domain.Entities.Category { Id = 2, CategoryName = "Test Category^2", UserId = 1 } },
                },
                new string("20220816")
            }
        };

        [Theory]
        [MemberData(nameof(Test_ExecuteRecurringRecord))]
        public async Task ExecuteRecurringRecords_ForExistRecords_ShouldCreateRecords
            (List<CreateCategoryCommand> createCategories, List<CreateRecurringRecordCommand> createRecurrings, List<RecordDto> resultRecords, string date)
        {
            await TestUtils.PostRecordsByList(_httpClient, createCategories, "/api/category");
            await TestUtils.PostRecordsByList(_httpClient, createRecurrings, "api/recurring");

            var response = await _httpClient.GetAsync($"api/recurring/ex/{date}");

            var response2 = await _httpClient.GetAsync("/api/tracker");
            var json = await response2.Content.ReadAsStringAsync();
            var returnedListOfRecords = JsonConvert.DeserializeObject<List<RecordDto>>(json);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            returnedListOfRecords.Should().BeEquivalentTo(resultRecords);
        }
    }
}
