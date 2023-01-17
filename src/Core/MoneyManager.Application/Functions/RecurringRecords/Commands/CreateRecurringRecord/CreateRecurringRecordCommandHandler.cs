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
        private readonly IMediator _mediator;

        public CreateRecurringRecordCommandHandler(IRecurringRecordRepository recurringRecordRepository, IMapper mapper, IMediator mediator)
        {
            _recurringRecordRepository = recurringRecordRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CreateRecurringRecordCommandResponse> Handle(CreateRecurringRecordCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateRecurringRecordCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return new CreateRecurringRecordCommandResponse(validatorResult);

            var recurringRecord = _mapper.Map<RecurringRecord>(request);

            recurringRecord = await _recurringRecordRepository.AddAsync(recurringRecord);

            if (request.TransactionDate is not null)
            {
                await _mediator.Send(new CreateRecordCommand { Name = request.Name, Amount = request.Amount, CategoryId = (int)request.CategoryId, TransactionDate = (DateTime)request.TransactionDate, UserId = request.UserId });
            }

            return new CreateRecurringRecordCommandResponse(recurringRecord.Id);
        }
    }
}
