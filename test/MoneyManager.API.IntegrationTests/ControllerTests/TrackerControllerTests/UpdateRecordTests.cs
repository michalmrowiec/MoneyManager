using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MoneyManager.API.IntegrationTests.ControllerTests.ControllerTestUtils;
using MoneyManager.API.IntegrationTests.ControllerTestUtils;
using MoneyManager.Application.Functions.Records;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new CreateRecordCommand { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                },
                new UpdateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, TransactionDate = new DateTime(2013, 08, 11), UserId = 1 },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -75M, TransactionDate = new DateTime(2013, 08, 11) },
                    new RecordDto { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new RecordDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordDto { Id = 5, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                }
            },
            new object[]
            {
                new List<CreateRecordCommand>
                {
                    new CreateRecordCommand { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new CreateRecordCommand { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new CreateRecordCommand { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new CreateRecordCommand { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new CreateRecordCommand { Id = 6, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new CreateRecordCommand { Id = 779, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new CreateRecordCommand { Id = 88, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                },
                new UpdateRecordCommand { Id = 779, Name = "TEST", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04), UserId = 1 },
                new List<RecordDto>
                {
                    new RecordDto { Id = 1, Name = "Tt", Amount = 79228162514264337593543950335M, TransactionDate = new DateTime(2020, 01, 01) },
                    new RecordDto { Id = 2, Name = "Alsfjtme 4352 ptor aksdfg", Amount = -79228162514264337593543950335M, TransactionDate = new DateTime(2013, 07, 11) },
                    new RecordDto { Id = 3, Name = "0000000", Amount = 0M, TransactionDate = new DateTime(1999, 12, 30) },
                    new RecordDto { Id = 4, Name = "!@#$%^&*()_+=", Amount = 1.01M },
                    new RecordDto { Id = 6, Name = "Test test test123", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new RecordDto { Id = 779, Name = "TEST", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) },
                    new RecordDto { Id = 88, Name = "Test", Amount = 8965.18M, TransactionDate = new DateTime(2019, 05, 04) }
                }
            }
        };

        [Theory]
        [MemberData(nameof(Test_ListOfRecordsToUpdate))]
        public async Task UpdateRecord_ForExistRecordAndValidData_ReturnsOkStatus
            (List<CreateRecordCommand> CreateRecordCommands, UpdateRecordCommand recordToUpdate, List<RecordDto> updatedRecords)
        {
            await TestUtils.PostRecordsByList(_httpClient, CreateRecordCommands, "/api/tracker");

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
