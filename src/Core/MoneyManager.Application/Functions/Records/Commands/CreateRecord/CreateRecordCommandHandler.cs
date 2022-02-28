using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.Records.Commands.CreateRecord;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Functions.Records
{
    public class CreateRecordCommandHandler : IRequestHandler<CreateRecordCommand, CreateRecordCommandResponse>
    {
        private readonly IRecordRepsitory _recordRepsitory;
        private readonly IMapper _mapper;

        public CreateRecordCommandHandler(IRecordRepsitory recordRepsitory, IMapper mapper)
        {
            _recordRepsitory = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<CreateRecordCommandResponse> Handle(CreateRecordCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateRecordCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return new CreateRecordCommandResponse(validatorResult);

            var record = _mapper.Map<Record>(request);

            record = await _recordRepsitory.AddAsync(record);

            return new CreateRecordCommandResponse(record.Id);
        }
    }
}
