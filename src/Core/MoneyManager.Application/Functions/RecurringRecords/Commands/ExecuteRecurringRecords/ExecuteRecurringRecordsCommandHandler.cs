using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords.Utils;
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
        private readonly IMediator _mediator;

        public ExecuteRecurringRecordsCommandHandler(IRecurringRecordRepository recurringRecordRepository, IMediator mediator)
        {
            _recurringRecordRepository = recurringRecordRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ExecuteRecurringRecordsCommand request, CancellationToken cancellationToken)
        {
            var recurringRecords = await _recurringRecordRepository.GetAllAsync(request.UserId);
            recurringRecords = recurringRecords.Where(x => x.IsActive == true).ToList();

            var executer = new RecurringRecordsExecuter();

            List<CreateRecordCommand> totalRecordToCreate = new();

            foreach (var recurringRecod in recurringRecords)
                if (recurringRecod.NextDate <= request.ComparisonDate)
                {
                    List<CreateRecordCommand> recordsToCreate = executer.GetListOfRecordsAndUpdateReccuringRecord(recurringRecod, request.ComparisonDate);
                    executer.UpdateNextDateForRecurringRecords(_mediator);
                    foreach (var record in recordsToCreate)
                        totalRecordToCreate.Add(record);
                }

            foreach (var record in totalRecordToCreate)
                await _mediator.Send(record); //here is some problem, sometimes it doesn't add (last record)

            return Unit.Value;
        }
    }
}
