using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.RecurringRecords.Queries.GetRecurringRecordById
{
    public class GetRecurringRecordByIdQueryHandler : IRequestHandler<GetRecurringRecordByIdQuery, RecurringRecordDto>
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public GetRecurringRecordByIdQueryHandler(IRecordRepository recordRepsitory, IMapper mapper)
        {
            _recordRepository = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<RecurringRecordDto> Handle(GetRecurringRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RecurringRecordDto>(await _recordRepository.GetByIdAsync(request.UserId, request.RecurringRecordId));
        }
    }
}
