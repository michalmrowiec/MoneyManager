using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Functions.Records
{
    public class UpdateRecordCommandHandler : IRequestHandler<UpdateRecordCommand>
    {
        private readonly IRecordRepsitory _recordRepository;
        private readonly IMapper _mapper;

        public UpdateRecordCommandHandler(IRecordRepsitory recordRepsitory, IMapper mapper)
        {
            _recordRepository = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateRecordCommand request, CancellationToken cancellationToken)
        {

            var record = _mapper.Map<Record>(request);

            await _recordRepository.UpdateAsync(record);

            return Unit.Value;
        }
    }
}
