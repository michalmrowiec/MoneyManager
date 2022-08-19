using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.RecurringRecordControllerTests
{
    public class CreateRecurringRecordTests : ControllerTestBase
    {
        public CreateRecurringRecordTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_CreateRecurringRecord => new List<object[]>
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
                }
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
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_CreateRecurringRecord))]
        public async Task CreateRecurringRecord_WithValidModel_ReturnsCreatedStatus
            (List<CreateCategoryCommand> createCategories, List<CreateRecurringRecordCommand> createRecurrings)
        {
            await TestUtils.PostRecordsByList(_httpClient, createCategories, "/api/category");
            await TestUtils.PostRecordsByList(_httpClient, createRecurrings, "api/recurring");

            var response = await _httpClient.GetAsync("api/recurring/ex/20220401");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
