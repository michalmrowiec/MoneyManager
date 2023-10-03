using MediatR;

namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail
{
    public record SendChangeEmailEmailCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }
    }
}
