using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Server.Queries
{
    public record GetRecordsForCategoryQuery(int Id) : IRequest<List<RecordItemDto>>;
}
