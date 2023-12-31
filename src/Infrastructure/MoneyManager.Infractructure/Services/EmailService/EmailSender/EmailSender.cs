using System.Net;
using System.Net.Mail;
using System.Text;

namespace MoneyManager.Infractructure.Services.EmailService.EmailSender
{
    internal class EmailSender : IEmailSender
    {
        private SmtpClient _smtp;
        private MailMessage _mail;

        private string _hostSmtp;
        private bool _enableSsl;
        private int _port;
        private string _senderEmail;
        private string _senderEmailPassword;
        private string _senderName;

        public EmailSender(EmailParams emailParams)
        {
            _hostSmtp = emailParams.HostSmtp;
            _enableSsl = emailParams.EnableSsl;
            _port = emailParams.Port;
            _senderEmail = emailParams.SenderEmail;
            _senderEmailPassword = emailParams.SenderEmailPassword;
            _senderName = emailParams.SenderName;
        }

        virtual public async Task SendEmailWithHtmlBody(string recipient, string subject, string body)
        {
            _mail = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                SubjectEncoding = Encoding.UTF8,
                Subject = subject,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Body = body
            };

            _mail.To.Add(new MailAddress(recipient));

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
