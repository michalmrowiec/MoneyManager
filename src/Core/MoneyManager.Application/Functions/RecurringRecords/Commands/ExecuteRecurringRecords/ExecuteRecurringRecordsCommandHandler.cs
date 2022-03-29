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

            var executer = new RecurringRecordsExecuter(_mediator);

            foreach (var recurringRecod in recurringRecords)
            {
                if (recurringRecod.NextDate <= DateTime.Today)
                {
                    List<CreateRecordCommand> recordsToCreate = executer.GetListOfRecordsAndUpdateReccuringRecord(recurringRecod);
                    foreach (var record in recordsToCreate)
                    {
                        await _mediator.Send(record);
                    }
                }
            }

            return Unit.Value;
        }
    }
}
