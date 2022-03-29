using Xunit;
using MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.UnitTests
{
    public class ExecuteRecurringRecordCommandHandlerTests
    {
        [Fact]
        public void HowManyTeest()
        {
            RecurringRecord recurringRecord = new();
            ExecuteRecurringRecordsCommandHandler.HowMany(recurringRecord);
        }
    }
}