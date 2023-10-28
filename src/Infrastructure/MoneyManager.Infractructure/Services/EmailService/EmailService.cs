using Microsoft.Extensions.Configuration;
using MoneyManager.Application.Contracts.Services;
using MoneyManager.Infractructure.Services.EmailService.EmailSender;
using Newtonsoft.Json.Linq;

namespace MoneyManager.Infractructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public EmailService(IEmailSender emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task SendChangeEmailEmailAsync(string recipient, string keyConfirmingEmailChange)
        {
            string url = _configuration["Url:ChangeEmail"] + keyConfirmingEmailChange;
            string htmlContent = "<a href=" + url + ">Confirm change emial</a>";

            await _emailSender.SendEmailWithHtmlBody(recipient, "Change email", htmlContent);
        }

        public async Task SendForgotPasswordEmailAsync(string recipient, string token)
        {
            string url = _configuration["Url:ForgotPassword"] + token;
            string htmlContent = "<a href=" + url + ">Reset password</a>";

            await _emailSender.SendEmailWithHtmlBody(recipient, "Reset password", htmlContent);
        }
    }
}
