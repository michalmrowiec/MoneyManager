namespace MoneyManager.Infractructure.Services.EmailService.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailWithHtmlBody(string recipient, string subject, string body);
    }
}
