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

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords
{
    public class ExecuteRecurringRecordsCommandHandler : IRequestHandler<ExecuteRecurringRecordsCommand>
    {
        private readonly IRecurringRecordRepository _recurringRecordRepository;

        public ExecuteRecurringRecordsCommandHandler(IRecurringRecordRepository recurringRecordRepository)
        {
            _recurringRecordRepository = recurringRecordRepository;
        }

        public async Task<Unit> Handle(ExecuteRecurringRecordsCommand request, CancellationToken cancellationToken)
        {
            var recurringRecords = await _recurringRecordRepository.GetAllAsync(request.UserId);
            recurringRecords = recurringRecords.Where(x => x.RecurringActive == true).ToList();

            foreach (var recurringRecod in recurringRecords)
            {
                if (recurringRecod.NextDate <= DateTime.Today)
                {
                    List<CreateRecordCommand> recordsToCreate = HowMany(recurringRecod);
                }
            }



            throw new NotImplementedException();
        }

        private List<CreateRecordCommand> HowMany(RecurringRecord recurringRecord)
        {
            List<CreateRecordCommand> listOfRecords = new();
            var recordWithNextDate = new CreateRecordCommand
            {
                Name = recurringRecord.Record.Name,
                CategoryName = recurringRecord.Record.Category.CategoryName,
                Amount = recurringRecord.Record.Amount,
                TransactionDate = recurringRecord.NextDate ?? DateTime.Now,
                CategoryId = recurringRecord.Record.CategoryId,
                UserId = recurringRecord.Record.UserId,
            };
            listOfRecords.Add(recordWithNextDate);
            var amountOfTimes = recurringRecord.AmountOfTimes - 1;


            if (recurringRecord.RepeatAfterDays != null && recurringRecord.NextDate != null)
            {
                double afterDays;
                DateTime nextDate = (DateTime)recurringRecord.NextDate;
                while (true)
                {
                    afterDays = (double)recurringRecord.RepeatAfterDays;

                    if (nextDate + TimeSpan.FromDays(afterDays) <= recurringRecord.EndDate)
                        nextDate += TimeSpan.FromDays(afterDays);
                    else
                    {
                        DezactiveRecurringRecord(recurringRecord, nextDate);
                        break;
                    }

                    if (nextDate <= DateTime.Today && amountOfTimes > 0)
                    {
                        var recordWithNextDateAgain = new CreateRecordCommand
                        {
                            Name = recurringRecord.Record.Name,
                            CategoryName = recurringRecord.Record.Category.CategoryName,
                            Amount = recurringRecord.Record.Amount,
                            TransactionDate = nextDate,
                            CategoryId = recurringRecord.Record.CategoryId,
                            UserId = recurringRecord.Record.UserId,
                        };
                        listOfRecords.Add(recordWithNextDateAgain);
                        amountOfTimes--;
                    }
                    else if (nextDate > DateTime.Today && amountOfTimes > 0)
                    {
                        SetNextDateOfRecurringRecord(recurringRecord, nextDate, amountOfTimes);
                    }
                    else
                    {
                        DezactiveRecurringRecord(recurringRecord, recurringRecord.EndDate);
                    }
                }
            }
            else if (recurringRecord.RepeatEveryDayOfMonth != null && recurringRecord.NextDate != null)
            {
                //var dayOfMonth = (int)recurringRecord.RepeatEveryDayOfMonth;
                //DateTime nextDate = (DateTime)recurringRecord.NextDate;
                //nextDate = nextDate.AddMonths(1);

                while (true)
                {
                    var dayOfMonth = (int)recurringRecord.RepeatEveryDayOfMonth;
                    DateTime nextDate = (DateTime)recurringRecord.NextDate;
                    //nextDate = nextDate.AddMonths(1);

                    if (nextDate.AddMonths(1) <= recurringRecord.EndDate)
                        nextDate = nextDate.AddMonths(1);
                    else
                    {
                        DezactiveRecurringRecord(recurringRecord, nextDate);
                        break;
                    }

                    if (nextDate <= DateTime.Today && amountOfTimes > 0)
                    {
                        var recordWithNextDateAgain = new CreateRecordCommand
                        {
                            Name = recurringRecord.Record.Name,
                            CategoryName = recurringRecord.Record.Category.CategoryName,
                            Amount = recurringRecord.Record.Amount,
                            TransactionDate = nextDate,
                            CategoryId = recurringRecord.Record.CategoryId,
                            UserId = recurringRecord.Record.UserId,
                        };
                        listOfRecords.Add(recordWithNextDateAgain);
                        amountOfTimes--;
                    }
                    else if (nextDate > DateTime.Today && amountOfTimes > 0)
                    {
                        SetNextDateOfRecurringRecord(recurringRecord, nextDate, amountOfTimes);
                    }
                    else
                    {
                        DezactiveRecurringRecord(recurringRecord, recurringRecord.EndDate);
                    }
                }
            }

            return listOfRecords;
        }

        private void DezactiveRecurringRecord(RecurringRecord recurringRecord, DateTime? lastDate)
        {
            var updateDezactivatedrecurringRecord = new RecurringRecord
            {
                Id = recurringRecord.Id,
                RecurringActive = false,
                Record =
                            {
                                Id = recurringRecord.Record.Id,
                                Name = recurringRecord.Record.Name,
                                Amount = recurringRecord.Record.Amount,
                                TransactionDate = recurringRecord.Record.TransactionDate,
                                CategoryId = recurringRecord.Record.CategoryId,
                                UserId = recurringRecord.Record.UserId,
                            },
                NextDate = null,
                RepeatAfterDays = recurringRecord.RepeatAfterDays,
                RepeatEveryDayOfMonth = recurringRecord.RepeatEveryDayOfMonth,
                EndDate = lastDate,
                AmountOfTimes = 0,
                UserId = recurringRecord.Record.UserId
            };
            _recurringRecordRepository.UpdateAsync(updateDezactivatedrecurringRecord);
        }

        private void SetNextDateOfRecurringRecord(RecurringRecord recurringRecord, DateTime? nextDate, int? amountOfTimes)
        {
            var recurringRecordWithUpdatedNextDate = new RecurringRecord
            {
                Id = recurringRecord.Id,
                RecurringActive = true,
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
                RepeatAfterDays = recurringRecord.RepeatAfterDays,
                RepeatEveryDayOfMonth = recurringRecord.RepeatEveryDayOfMonth,
                EndDate = recurringRecord.EndDate,
                AmountOfTimes = amountOfTimes,
                UserId = recurringRecord.Record.UserId
            };
            _recurringRecordRepository.UpdateAsync(recurringRecordWithUpdatedNextDate);
        }
    }
}
