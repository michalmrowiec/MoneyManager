using Xunit;
using MoneyManager.Domain.Entities;
using MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords.Utils;
using System;
using System.Collections.Generic;
using FluentAssertions;
using MoneyManager.Application.Functions.Records;
using Record = MoneyManager.Domain.Entities.Record;

namespace MoneyManager.Application.UnitTests
{
    public class RecurringRecordsExecuterTests
    {
        public static IEnumerable<object[]> RecurringRecord => new List<object[]>
        {
            new object[]
            {
                new RecurringRecord
                {
                    Id = 1,
                    IsActive = true,
                    NextDate = new DateTime(2022, 01, 02),
                    Name = "Test",
                    CategoryId = 12,
                    Amount = 100,
                    TransactionDate = new DateTime(2020, 01, 01),
                    RepeatEveryDayOfMonth = 2,
                    UserId = 1,
                },
                new List<Record>
                {
                    new Record
                    { Name = "Test", CategoryId = 12, Amount = 100, TransactionDate = new DateTime(2022, 01, 02), UserId = 1 },
                    new Record
                    { Name = "Test", CategoryId = 12, Amount = 100, TransactionDate = new DateTime(2022, 02, 02), UserId = 1 },
                    new Record
                    { Name = "Test", CategoryId = 12, Amount = 100, TransactionDate = new DateTime(2022, 03, 02), UserId = 1 },
                }
            }
        };

        [Theory]
        [MemberData(nameof(RecurringRecord))]
        public void HowManyTest(RecurringRecord recurringRecord, List<Record> expectedReturnedList)
        {
            RecurringRecordsExecuter recurringRecordsExecuter = new();
            var result = recurringRecordsExecuter.GetListOfRecordsAndUpdateReccuringRecord(recurringRecord, new DateTime(2022, 03, 03));

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(expectedReturnedList);
        }
    }
}