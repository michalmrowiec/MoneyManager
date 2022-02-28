using MediatR;

namespace MoneyManager.Application.Functions.Records
{
    public record GetAllRecordsQuery(int UserId) : IRequest<List<RecordDto>>;
}
