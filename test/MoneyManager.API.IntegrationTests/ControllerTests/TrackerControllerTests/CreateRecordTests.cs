using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.Application.Functions.Records;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MoneyManager.API.IntegrationTests.ControllerTests.TrackerControllerTests
{
    public class CreateRecordTests : ControllerTestBase
    {
        public CreateRecordTests(WebApplicationFactory<Startup> factory) : base(factory)
        { }

        public static IEnumerable<object[]> RecordsWithValidData => new List<object[]>
        {
            new object[] { new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) } },
            new object[] { new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) } },
            new object[] { new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) } },
            new object[] { new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M } },
            new object[] { new CreateRecordCommand { Id = 0, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) } }
        };

        public static IEnumerable<object[]> RecordsWithInvalidData => new List<object[]>
        {
            new object[]
            {
                new CreateRecordCommand { Id = 1, Name = "T", Amount = 7922816251426433759335M, TransactionDate = new DateTime(2020, 01, 01) },
                null,
                false,
                3,
                new List<string> { "Name must be at least 2 characters long" }
            },
            new object[]
            {
                new CreateRecordCommand { Id = 1, Name = "", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                null,
                false,
                3,
                new List<string> { "Name is required", "Name must be at least 2 characters long" }
            }
        };

        [Theory]
        [MemberData(nameof(RecordsWithValidData))]
        public async Task CreateRecord_WithValidModel_ReturnsCreatedStatus(CreateRecordCommand record)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/tracker", httpContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        // TODO: Not BadRequest hehe... suprise
        [Theory]
        [MemberData(nameof(RecordsWithInvalidData))]
        public async Task CreateRecord_WithInvalidModel_ReturnsBadRequestStatus(CreateRecordCommand record, int? exRecordId, bool exSuccess, int exStatus, List<string> exValidationErrors)
        {
            var json = JsonConvert.SerializeObject(record);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/tracker", httpContent);

            var resString = await response.Content.ReadAsStringAsync();
            dynamic? baseResponse = JsonConvert.DeserializeObject<dynamic>(resString);
            var recordId = (int?)baseResponse?.recordId;
            bool success = Convert.ToBoolean(baseResponse?.success);
            int status = Convert.ToInt32(baseResponse?.status);
            List<string>? validationErrors = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(baseResponse?.validationErrors));

            recordId.Should().Be(exRecordId);
            success.Should().Be(exSuccess);
            status.Should().Be(exStatus);
            validationErrors.Should().BeEquivalentTo(exValidationErrors);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }
    }
}
