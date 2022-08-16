using MediatR;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Records.Commands.CreateRangeRecords
{
    public record CreateRangeRecordsCommand(Record[] records) : IRequest;
}
