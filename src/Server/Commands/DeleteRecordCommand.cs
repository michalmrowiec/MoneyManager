using MediatR;

namespace MoneyManager.Server.Commands
{
    public record DeleteRecordCommand(int Id) : IRequest;
}
