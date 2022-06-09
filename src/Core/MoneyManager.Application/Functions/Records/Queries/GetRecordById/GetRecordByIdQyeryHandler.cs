using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Records.Queries.GetRecordById
{
    public class GetRecordByIdQyeryHandler : IRequestHandler<GetRecordByIdQuery, RecordDto>
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public GetRecordByIdQyeryHandler(IRecordRepository recordRepsitory, IMapper mapper)
        {
            _recordRepository = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<RecordDto> Handle(GetRecordByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RecordDto>(await _recordRepository.GetByIdAsync(request.UserId, request.RecordId));
        }
    }
}
