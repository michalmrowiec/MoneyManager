using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Records.Queries.GetRecordById
{
    public record GetRecordByIdQuery(int UserId, int RecordId) : IRequest<RecordDto>;
}
