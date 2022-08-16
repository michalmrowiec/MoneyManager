using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.Application.Functions.Records;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.TrackerControllerTests
{
    public class DeleteRecordTests : ControllerTestBase
    {
        public DeleteRecordTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> Test_SingleRecords => new List<object[]>
        {
            new object[] { new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) } },
            new object[] { new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) } },
            new object[] { new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) } },
            new object[] { new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M } },
            new object[] { new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) } }
        };

        [Theory]
        [MemberData(nameof(Test_SingleRecords))]
        public async Task DeleteRecord_ForExistRecord_ReturnsNoContentStatus(CreateRecordCommand record)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await _httpClient.PostAsync("/api/tracker", httpContent);

            var response = await _httpClient.DeleteAsync($"/api/tracker/{record.Id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
