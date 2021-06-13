namespace EventDriven.Domain.PoC.Application.DomainServices.EmailServices
{
    public interface IEmailService
    {
        /// <summary>
        ///     Send an email via gMail, the settings (username, password, server, port) are read from the appsettings.json file
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="html"></param>
        /// <param na
        void Send(string to, string subject, string html, string from = null);
    }
}