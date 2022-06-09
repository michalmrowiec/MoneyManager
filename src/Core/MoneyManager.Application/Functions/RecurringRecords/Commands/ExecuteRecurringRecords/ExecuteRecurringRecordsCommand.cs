using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords
{
    public record ExecuteRecurringRecordsCommand(int UserId, DateTime ComparisonDate) : IRequest;
}
