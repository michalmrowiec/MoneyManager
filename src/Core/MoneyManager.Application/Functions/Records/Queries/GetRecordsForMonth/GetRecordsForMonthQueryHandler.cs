using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Records.Queries.GetRecordsForMonth
{
    public class GetRecordsForMonthQueryHandler : IRequestHandler<GetRecordsForMonthQuery, List<RecordDto>>
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public GetRecordsForMonthQueryHandler(IRecordRepository recordRepsitory, IMapper mapper)
        {
            _recordRepository = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<List<RecordDto>> Handle(GetRecordsForMonthQuery request, CancellationToken cancellationToken)
        {
            var records = _mapper.Map<List<RecordDto>>
                (await _recordRepository.GetRecordsForMonth(request.UserId, request.Year, request.Month));

            return records.Count != 0 ? records : new();
        }
    }
}
