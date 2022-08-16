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
        private List<RecurringRecord> _recurringRecordsToUpdate = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recurringRecord"></param>
        /// <param name="referenceDate">Schould be DataTime.Today or DataTime.Now</param>
        /// <returns>List of records for past months</returns>
        internal List<Record> GetListOfRecordsAndUpdateReccuringRecord(RecurringRecord recurringRecord, DateTime referenceDate)
        {
            List<Record> listOfRecords = new();
            listOfRecords.Add(new Record
            {
                Name = recurringRecord.Name,
                //CategoryName = recurringRecord.Category.CategoryName,
                Amount = recurringRecord.Amount,
                TransactionDate = recurringRecord.NextDate,
                CategoryId = recurringRecord.CategoryId,
                UserId = recurringRecord.UserId
            });
            //var recordWithNextDate = new CreateRecordCommand
            //{
            //    Name = recurringRecord.Record.Name,
            //    CategoryName = recurringRecord.Record.Category.CategoryName,
            //    Amount = recurringRecord.Record.Amount,
            //    TransactionDate = recurringRecord.NextDate,
            //    CategoryId = recurringRecord.Record.CategoryId,
            //    UserId = recurringRecord.UserId,
            //};
            //listOfRecords.Add(recordWithNextDate);


            DateTime nextDate = recurringRecord.NextDate;

            while (true)
            {
                nextDate = nextDate.AddMonths(1);

                if (nextDate <= referenceDate)
                {
                    listOfRecords.Add(new Record
                    {
                        Name = recurringRecord.Name,
                        //CategoryName = recurringRecord.Category.CategoryName,
                        Amount = recurringRecord.Amount,
                        TransactionDate = nextDate,
                        CategoryId = recurringRecord.CategoryId,
                        UserId = recurringRecord.UserId,
                    });
                }
                else
                {
                    _recurringRecordsToUpdate.Add(new RecurringRecord
                    {
                        Id = recurringRecord.Id,
                        IsActive = recurringRecord.IsActive,
                        NextDate = nextDate,
                        RepeatEveryDayOfMonth = recurringRecord.RepeatEveryDayOfMonth,
                        Name = recurringRecord.Name,
                        Amount = recurringRecord.Amount,
                        TransactionDate = recurringRecord.TransactionDate,
                        CategoryId = recurringRecord.CategoryId,
                        UserId = recurringRecord.UserId
                    });
                    break;
                }
            }

            return listOfRecords;
        }

        internal void UpdateNextDateForRecurringRecords(IMediator mediator)
        {
            foreach (var recurringRecord in _recurringRecordsToUpdate)
            {
                var recurringRecordWithUpdatedNextDate = new UpdateRecurringRecordCommand
                {
                    Id = recurringRecord.Id,
                    IsActive = true,
                    Name = recurringRecord.Name,
                    Amount = recurringRecord.Amount,
                    TransactionDate = recurringRecord.TransactionDate,
                    CategoryId = recurringRecord.CategoryId,
                    NextDate = recurringRecord.NextDate,
                    RepeatEveryDayOfMonth = recurringRecord.RepeatEveryDayOfMonth,
                    UserId = recurringRecord.UserId
                };
                mediator.Send(recurringRecordWithUpdatedNextDate);
            }
        }
    }
}
