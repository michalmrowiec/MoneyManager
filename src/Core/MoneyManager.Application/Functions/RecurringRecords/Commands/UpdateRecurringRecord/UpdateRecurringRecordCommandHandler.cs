using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.UpdateRecurringRecord
{
    public class UpdateRecurringRecordCommandHandler : IRequestHandler<UpdateRecurringRecordCommand>
    {
        private readonly IRecurringRecordRepository _recurringRecordRepository;
        private readonly IMapper _mapper;

        public UpdateRecurringRecordCommandHandler(IRecurringRecordRepository recurringRecordRepository, IMapper mapper)
        {
            _recurringRecordRepository = recurringRecordRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateRecurringRecordCommand request, CancellationToken cancellationToken)
        {
            var recurringRecord = _mapper.Map<RecurringRecord>(request);
            await _recurringRecordRepository.UpdateAsync(recurringRecord);
            return Unit.Value;
        }
    }
}
