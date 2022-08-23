using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.PlannedRecordControllerTests
{
    public class GetPlannedBudgetsTests : ControllerTestBase
    {
        public GetPlannedBudgetsTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_GetPlannedBudgets => new List<object[]>
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
                    new CreatePlannedBudgetCommand { Id = 3, Amount = 89.23M, CategoryId = 2, PlanForMonth = new DateTime(2021, 12, 01) },
                    new CreatePlannedBudgetCommand { Id = 4, Amount = 6222M, CategoryId = 2, PlanForMonth = new DateTime(2001, 04, 01) }
                },
                new int[] { 2001 },
                new int[] { 4 },
                new List<PlannedBudgetDto>
                {
                    new PlannedBudgetDto { Id = 1, Amount = 1040M, CategoryId = 1, PlanForMonth = new DateTime(2001, 04, 01), UserId = 1 },
                    new PlannedBudgetDto { Id = 4, Amount = 6222M, CategoryId = 2, PlanForMonth = new DateTime(2001, 04, 01), UserId = 1 },
                },
            }
        };

        [Theory]
        [MemberData(nameof(Test_GetPlannedBudgets))]
        public async Task GetPlannedBudgetsForMonth_ForValidData_ReturnsListOfPlannedBudgetsForGivenMonth
            (List<CreateCategoryCommand> createCategories, List<CreatePlannedBudgetCommand> createPlannedBudgets,
            int[] year, int[] month, List<PlannedBudgetDto> expectedResult)
        {
            await TestUtils.PostItemsByListAsync(_httpClient, createCategories, "/api/category");
            await TestUtils.PostItemsByListAsync(_httpClient, createPlannedBudgets, "/api/plannedbudget");

            var responseMessage = await _httpClient.GetAsync($"/api/plannedbudget/{year[0]}/{month[0]}");
            var jsonResponse = await responseMessage.Content.ReadAsStringAsync();
            var plannedBudgetsDtos = JsonConvert.DeserializeObject<List<PlannedBudgetDto>>(jsonResponse);

            responseMessage.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            plannedBudgetsDtos.Should().BeEquivalentTo(expectedResult);
        }
    }
}
