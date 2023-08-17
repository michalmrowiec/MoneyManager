namespace MoneyManager.Application.Contracts.Services
{
    public interface IEmailSender
    {
        Task SendForgotPasswordEmailAsync(string urlToResetPassword, string to);
    }
}
