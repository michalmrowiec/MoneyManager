using MoneyManager.Shared;
using MediatR;

namespace MoneyManager.Server.Commands
{
    public record UpdateRecordCommand(RecordItemDto RecordItemDto) : IRequest;
}
