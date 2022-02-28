using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Server.Queries
{
    public record GetAllRecordsQuery : IRequest<List<RecordItemDto>>;
}
