using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EventDriven.Domain.PoC.Application.DomainServices.EmailServices
{
    /// <summary>
    ///     Sends an email using g-mail settings provided via appsettings.json
    /// </summary>
    public class GmailService : IEmailService
    {
        private readonly MyConfigurationValues _appSettings;

        public GmailService(IOptions<MyConfigurationValues> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        ///     Send an email via gMail, the settings (username, password, server, port) are read from the appsettings.json file
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="html"></param>
        /// <param name="from"></param>
        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.SmtpOptions.EmailIsFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) {Text = html};

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpOptions.Host, _appSettings.SmtpOptions.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpOptions.UserName, _appSettings.SmtpOptions.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}