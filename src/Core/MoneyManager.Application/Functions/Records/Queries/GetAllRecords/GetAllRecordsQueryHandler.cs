using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.Records
{
    public class GetAllRecordsQueryHandler : IRequestHandler<GetAllRecordsQuery, List<RecordDto>>
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IMapper _mapper;

        public GetAllRecordsQueryHandler(IRecordRepository recordRepsitory, IMapper mapper)
        {
            _recordRepository = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<List<RecordDto>> Handle(GetAllRecordsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<RecordDto>>(await _recordRepository.GetAllRecordsAsync(request.UserId));
        }
    }
}
