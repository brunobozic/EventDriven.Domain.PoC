namespace EventDriven.Domain.PoC.SharedKernel.Helpers.EmailSender
{
    public class SmtpOptions
    {
        public int Port { get; set; }

        public string Host { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool EnableSsl { get; set; }
        public string EmailIsFrom { get; set; }
    }
}