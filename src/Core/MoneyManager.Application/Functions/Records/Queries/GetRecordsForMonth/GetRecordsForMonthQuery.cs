using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Records.Queries.GetRecordsForMonth
{
    public record GetRecordsForMonthQuery(int UsesrId, int Year, int Month) : IRequest<List<RecordDto>>;
}
