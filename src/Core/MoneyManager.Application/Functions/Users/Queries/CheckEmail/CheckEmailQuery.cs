using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Queries.CheckEmail
{
    public record CheckEmailQuery(string Email) : IRequest<bool>;
}
