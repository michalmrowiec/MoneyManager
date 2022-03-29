using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord
{
    public class CreateRecurringRecordCommandHandler : IRequestHandler<CreateRecurringRecordCommand, CreateRecurringRecordCommandResponse>
    {
        private readonly IRecurringRecordRepository _recurringRecordRepository;
        private readonly IMapper _mapper;

        public CreateRecurringRecordCommandHandler(IRecurringRecordRepository recurringRecordRepository, IMapper mapper)
        {
            _recurringRecordRepository = recurringRecordRepository;
            _mapper = mapper;
        }

        public async Task<CreateRecurringRecordCommandResponse> Handle(CreateRecurringRecordCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateRecurringRecordCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return new CreateRecurringRecordCommandResponse(validatorResult);

            var recurringRecord = _mapper.Map<RecurringRecord>(request);

            recurringRecord = await _recurringRecordRepository.AddAsync(recurringRecord);

            return new CreateRecurringRecordCommandResponse(recurringRecord.Id);
        }
    }
}
