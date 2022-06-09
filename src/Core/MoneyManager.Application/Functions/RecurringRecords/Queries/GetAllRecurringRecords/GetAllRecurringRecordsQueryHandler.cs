using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Queries.GetAllRecurringRecords
{
    public class GetAllRecurringRecordsQueryHandler : IRequestHandler<GetAllRecurringRecordsQuery, List<RecurringRecordDto>>
    {
        private readonly IRecurringRecordRepository _recurringRecordRepository;
        private readonly IMapper _mapper;

        public GetAllRecurringRecordsQueryHandler(IRecurringRecordRepository recurringRecordRepository, IMapper mapper)
        {
            _recurringRecordRepository = recurringRecordRepository;
            _mapper = mapper;
        }

        public async Task<List<RecurringRecordDto>> Handle(GetAllRecurringRecordsQuery request, CancellationToken cancellationToken)
        {
            var all = await _recurringRecordRepository.GetAllAsync(request.UserId);
            var mapped = _mapper.Map<List<RecurringRecordDto>>(all);
            return mapped;
        }
    }
}
