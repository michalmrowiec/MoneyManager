using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.Records.Commands.CreateRangeRecords
{
    public class CreateRangeRecordsCommandHandler : IRequestHandler<CreateRangeRecordsCommand>
    {
        private readonly IRecordRepository _recordRepsitory;

        public CreateRangeRecordsCommandHandler(IRecordRepository recordRepsitory)
        {
            _recordRepsitory = recordRepsitory;
        }
        public async Task<Unit> Handle(CreateRangeRecordsCommand request, CancellationToken cancellationToken)
        {
            await _recordRepsitory.AddRangeRecordAsync(request.records);

            return Unit.Value;
        }
    }
}
