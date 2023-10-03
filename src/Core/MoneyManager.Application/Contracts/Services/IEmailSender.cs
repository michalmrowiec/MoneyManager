namespace MoneyManager.Application.Contracts.Services
{
    public interface IEmailSender
    {
        Task SendForgotPasswordEmailAsync(string urlToResetPassword, string recipient);
        Task SendChangeEmialEmailAsync(string ulrToChangeEmail, string recipient);
    }
}
