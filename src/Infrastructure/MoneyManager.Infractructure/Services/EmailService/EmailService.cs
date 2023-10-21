using MoneyManager.Application.Contracts.Services;
using MoneyManager.Infractructure.Services.EmailService.EmailSender;

namespace MoneyManager.Infractructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendChangeEmailEmailAsync(string recipient, string keyConfirmingEmailChange)
        {
            string rer = @"https://www.moneymanager.hostingasp.pl/confirm-change-email?&keyConfirmingEmailChange=" + keyConfirmingEmailChange;
            string url = "<a href=" + rer + ">Confirm change emial</a>";

            await _emailSender.SendEmailWithHtmlBody(recipient, "Change email", url);
        }

        public async Task SendForgotPasswordEmailAsync(string recipient, string token)
        {
            string rer = @"https://www.moneymanager.hostingasp.pl/forgotpassword?&access_token=" + token;
            string url = "<a href=" + rer + ">Reset password</a>";

            await _emailSender.SendEmailWithHtmlBody(recipient, "Reset password", url);
        }
    }
}
