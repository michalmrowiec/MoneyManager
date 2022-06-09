using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Queries.GetRecurringRecordById
{
    public record GetRecurringRecordByIdQuery(int UserId, int RecurringRecordId) : IRequest<RecurringRecordDto>;
}
