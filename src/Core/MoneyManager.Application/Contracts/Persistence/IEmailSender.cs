namespace MoneyManager.Application.Contracts.Persistence
{
    public interface IEmailSender
    {
        Task SendForgotPasswordEmailAsync(string urlToResetPassword, string to);
    }
}
