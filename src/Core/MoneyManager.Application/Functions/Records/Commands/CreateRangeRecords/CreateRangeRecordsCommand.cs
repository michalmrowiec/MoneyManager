using MediatR;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Functions.Records.Commands.CreateRangeRecords
{
    public record CreateRangeRecordsCommand(Record[] records) : IRequest;
}
