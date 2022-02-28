using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.Records
{
    public class GetRecordsForCategoryQueryHandler : IRequestHandler<GetRecordsForCategoryQuery, List<RecordDto>>
    {
        private readonly IRecordRepsitory _recordRepository;
        private readonly IMapper _mapper;

        public GetRecordsForCategoryQueryHandler(IRecordRepsitory recordRepsitory, IMapper mapper)
        {
            _recordRepository = recordRepsitory;
            _mapper = mapper;
        }

        public async Task<List<RecordDto>> Handle(GetRecordsForCategoryQuery request, CancellationToken cancellationToken)
        {
            var records = await _recordRepository.GetRecordsForCategory(request.UserId, request.CategoryId);

            return _mapper.Map<List<RecordDto>>(records);
        }
    }
}
