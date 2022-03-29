using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.RecurringRecords.Commands.UpdateRecurringRecord;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords.Utils
{
    /// <summary>
    /// Require IMediator for update NextDate for Recurring Record
    /// </summary>
    internal class RecurringRecordsExecuter
    {
        private readonly IMediator _mediator;

        public RecurringRecordsExecuter(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recurringRecord"></param>
        /// <returns>List of records for past months</returns>
        internal List<CreateRecordCommand> GetListOfRecordsAndUpdateReccuringRecord(RecurringRecord recurringRecord)
        {
            List<CreateRecordCommand> listOfRecords = new();
            var recordWithNextDate = new CreateRecordCommand
            {
                Name = recurringRecord.Record.Name,
                CategoryName = recurringRecord.Record.Category.CategoryName,
                Amount = recurringRecord.Record.Amount,
                TransactionDate = recurringRecord.NextDate,
                CategoryId = recurringRecord.Record.CategoryId,
                UserId = recurringRecord.Record.UserId,
            };
            listOfRecords.Add(recordWithNextDate);


            DateTime nextDate = recurringRecord.NextDate;

            while (true)
            {
                nextDate = nextDate.AddMonths(1);

                if (nextDate <= DateTime.Today)
                {
                    recordWithNextDate.TransactionDate = nextDate;
                    listOfRecords.Add(recordWithNextDate);
                }
                else
                {
                    UpdateNextDateForRecurringRecord(recurringRecord, nextDate);
                    break;
                }
            }

            return listOfRecords;
        }

        private void UpdateNextDateForRecurringRecord(RecurringRecord recurringRecord, DateTime nextDate)
        {
            var recurringRecordWithUpdatedNextDate = new UpdateRecurringRecordCommand
            {
                Id = recurringRecord.Id,
                IsActive = true,
                Record =
                            {
                                Id = recurringRecord.Record.Id,
                                Name = recurringRecord.Record.Name,
                                Amount = recurringRecord.Record.Amount,
                                TransactionDate = recurringRecord.Record.TransactionDate,
                                CategoryId = recurringRecord.Record.CategoryId,
                                UserId = recurringRecord.Record.UserId,
                            },
                NextDate = nextDate,
                RepeatEveryDayOfMonth = recurringRecord.RepeatEveryDayOfMonth,
                UserId = recurringRecord.Record.UserId
            };
            _mediator.Send(recurringRecordWithUpdatedNextDate);
        }
    }
}
