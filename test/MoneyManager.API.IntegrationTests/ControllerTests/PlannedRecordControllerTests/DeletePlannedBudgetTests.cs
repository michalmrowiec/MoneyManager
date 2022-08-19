using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.PlannedRecordControllerTests
{
    public class DeletePlannedBudgetTests : ControllerTestBase
    {
        public DeletePlannedBudgetTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_SinglePlannedBudget => new List<object[]>
        {
            new object[] { new CreatePlannedBudgetCommand { Id = 1, Amount = 100M, PlanForMonth = new DateTime(2010, 09, 01) } },
            new object[] { new CreatePlannedBudgetCommand { Id = 2, Amount = 3213M, PlanForMonth = new DateTime(2018, 06, 01) } },
            new object[] { new CreatePlannedBudgetCommand { Id = 3, Amount = 89.23M, PlanForMonth = new DateTime(2022, 11, 01) } }
        };

        [Theory]
        [MemberData(nameof(Test_SinglePlannedBudget))]
        public async Task DeletePlannedBudget_ForExistPlannedBudgets_ReturnsNoContentStatus(CreatePlannedBudgetCommand plannedBudget)
        {
            var json = JsonConvert.SerializeObject(plannedBudget);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/plannedbudget", httpContent);

            var response = await _httpClient.DeleteAsync($"/api/plannedbudget/{plannedBudget.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
