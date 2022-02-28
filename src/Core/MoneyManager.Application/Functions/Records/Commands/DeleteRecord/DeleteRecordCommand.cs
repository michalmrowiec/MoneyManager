using MediatR;

namespace MoneyManager.Application.Functions.Records
{
    public record DeleteRecordCommand(int UserId, int Id) : IRequest;
}
