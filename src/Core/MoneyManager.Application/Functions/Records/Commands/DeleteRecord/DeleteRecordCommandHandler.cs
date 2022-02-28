using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.Records
{
    public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand>
    {
        private readonly IRecordRepsitory _recordRepsitory;

        public DeleteRecordCommandHandler(IRecordRepsitory recordRepsitory)
        {
            _recordRepsitory = recordRepsitory;
        }

        public async Task<Unit> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
        {
            var recordDelete = await _recordRepsitory.GetByIdAsync(request.UserId, request.Id);

            await _recordRepsitory.DeleteAsync(request.UserId, recordDelete);

            return Unit.Value;
        }
    }
}
