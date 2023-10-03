using System.Net;
using System.Net.Mail;
using System.Text;
using MoneyManager.Application.Contracts.Services;

namespace MoneyManager.Infractructure.Services.EmailService
{
    public class Email : IEmailSender
    {
        private SmtpClient _smtp;
        private MailMessage _mail;

        private string _hostSmtp;
        private bool _enableSsl;
        private int _port;
        private string _senderEmail;
        private string _senderEmailPassword;
        private string _senderName;

        public Email(EmailParams emailParams)
        {
            _hostSmtp = emailParams.HostSmtp;
            _enableSsl = emailParams.EnableSsl;
            _port = emailParams.Port;
            _senderEmail = emailParams.SenderEmail;
            _senderEmailPassword = emailParams.SenderEmailPassword;
            _senderName = emailParams.SenderName;
        }

        public async Task SendChangeEmialEmailAsync(string ulrToChangeEmail, string recipient)
        {
            _mail = new MailMessage();

            _mail.From = new MailAddress(_senderEmail, _senderName);
            _mail.To.Add(new MailAddress(recipient));

            _mail.SubjectEncoding = Encoding.UTF8;
            _mail.Subject = "Reset Password";

            _mail.BodyEncoding = Encoding.UTF8;
            _mail.IsBodyHtml = true;
            _mail.Body = ulrToChangeEmail;

            _smtp = new SmtpClient
            {
                Host = _hostSmtp,
                EnableSsl = _enableSsl,
                Port = _port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderEmail, _senderEmailPassword)
            };

            _smtp.SendCompleted += OnSendCompleted;

            await _smtp.SendMailAsync(_mail);
        }

        public async Task SendForgotPasswordEmailAsync(string urlToResetPassword, string to)
        {
            _mail = new MailMessage();

            _mail.From = new MailAddress(_senderEmail, _senderName);
            _mail.To.Add(new MailAddress(to));

            _mail.SubjectEncoding = Encoding.UTF8;
            _mail.Subject = "Reset Password";

            _mail.BodyEncoding = Encoding.UTF8;
            _mail.IsBodyHtml = true;
            _mail.Body = urlToResetPassword;

            _smtp = new SmtpClient
            {
                Host = _hostSmtp,
                EnableSsl = _enableSsl,
                Port = _port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderEmail, _senderEmailPassword)
            };

            _smtp.SendCompleted += OnSendCompleted;

            await _smtp.SendMailAsync(_mail);
        }

        private void OnSendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            _smtp.Dispose();
            _mail.Dispose();
        }
    }
}
