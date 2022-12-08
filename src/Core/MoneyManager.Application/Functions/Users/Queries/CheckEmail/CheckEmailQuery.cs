using MediatR;

namespace MoneyManager.Application.Functions.Users.Queries.CheckEmail
{
    public record CheckEmailQuery(string Email) : IRequest<bool>;
}
