using MediatR;
using MoneyManager.Application.Functions.Records;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetCurrentTotalAmount
{
    public class GetCurrentTotalAmountQueryHandler : IRequestHandler<GetCurrentTotalAmountQuery, decimal>
    {
        private readonly IMediator _mediator;
        public GetCurrentTotalAmountQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<decimal> Handle(GetCurrentTotalAmountQuery request, CancellationToken cancellationToken)
        {
            var respond = await _mediator.Send(new GetAllRecordsQuery(request.UserId));
            return respond.Sum(x => x.Amount);
        }
    }
}
