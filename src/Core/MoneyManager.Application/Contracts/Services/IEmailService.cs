namespace MoneyManager.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task SendForgotPasswordEmailAsync(string recipient, string token);
        Task SendChangeEmailEmailAsync(string recipient, string keyConfirmingEmailChange);
    }
}
