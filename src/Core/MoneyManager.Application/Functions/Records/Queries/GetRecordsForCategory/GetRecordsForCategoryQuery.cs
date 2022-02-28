using MediatR;

namespace MoneyManager.Application.Functions.Records
{
    public record GetRecordsForCategoryQuery(int UserId, int CategoryId) : IRequest<List<RecordDto>>;
}
