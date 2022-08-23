using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Commands.UpdatePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.PlannedRecordControllerTests
{
    public class UpdatePlannedBudgetTests : ControllerTestBase
    {
        public UpdatePlannedBudgetTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_UpdatePlannedBudget => new List<object[]>
        {
            new object[]
            {
                new List<CreateCategoryCommand>
                {
                    new CreateCategoryCommand { Id = 1, Name = "Test1" },
                    new CreateCategoryCommand { Id = 2, Name = "!@#$%" },
                    new CreateCategoryCommand { Id = 3, Name = "23" },
                    new CreateCategoryCommand { Id = 4, Name = "SDg DFG  554%" }
                },
                new List<CreatePlannedBudgetCommand>
                {
                    new CreatePlannedBudgetCommand { Id = 1, Amount = 1040M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01) },
                    new CreatePlannedBudgetCommand { Id = 2, Amount = 3M, CategoryId = 4, PlanForMonth = new DateTime(2012, 06, 01) },
                    new CreatePlannedBudgetCommand { Id = 3, Amount = 89.23M, CategoryId = 2, PlanForMonth = new DateTime(2021, 12, 01) }
                },
                new UpdatePlannedBudgetCommand { Id = 3, Amount = 89.29M, CategoryId = 3, PlanForMonth = new DateTime(2021, 12, 01) },
                new List<PlannedBudgetDto>
                {
                    new PlannedBudgetDto { Id = 1, Amount = 1040M, FilledAmount = 0M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01), UserId = 1 },
                    new PlannedBudgetDto { Id = 2, Amount = 3M, FilledAmount = 0M, CategoryId = 4, PlanForMonth = new DateTime(2012, 06, 01), UserId = 1 },
                    new PlannedBudgetDto { Id = 3, Amount = 89.29M, FilledAmount = 0M, CategoryId = 3, PlanForMonth = new DateTime(2021, 12, 01), UserId = 1 }
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_UpdatePlannedBudget))]
        public async Task UpdatePlannedBudget_ForExistRexordsAndValidData_ReturnsOkStatus
            (List<CreateCategoryCommand> createCategories, List<CreatePlannedBudgetCommand> createPlannedBudgets,
            UpdatePlannedBudgetCommand updatePlannedBudget, List<PlannedBudgetDto> updatedPlannedBudgets)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, createCategories, "/api/category");
            await TestUtils.PostItemsByListAsync(_httpClient, createPlannedBudgets, "/api/plannedbudget");

            var jsonUpdate = JsonConvert.SerializeObject(updatePlannedBudget);
            var httpContentUpdate = new StringContent(jsonUpdate, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/plannedbudget", httpContentUpdate);

            var responseGetAllPlannedBudgets = await _httpClient.GetAsync("/api/plannedbudget");
            var jsonResponseGetAllPlannedBudgets = await responseGetAllPlannedBudgets.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<List<PlannedBudgetDto>>(jsonResponseGetAllPlannedBudgets);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            records.Should().BeEquivalentTo(updatedPlannedBudgets);
        }
    }
}
