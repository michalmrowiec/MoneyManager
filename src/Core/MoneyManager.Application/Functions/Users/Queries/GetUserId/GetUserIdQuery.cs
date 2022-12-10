using MediatR;

namespace MoneyManager.Application.Functions.Users.Queries.GetUserId
{
    public record GetUserIdQuery(string UserEmail) : IRequest<int?>;
}
