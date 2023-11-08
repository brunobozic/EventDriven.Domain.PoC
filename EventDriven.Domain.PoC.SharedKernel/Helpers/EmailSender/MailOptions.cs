namespace EventDriven.Domain.PoC.SharedKernel.Helpers.EmailSender
{
    public class MailOptions
    {
        public string ConfirmapplicationUserUrlTemplate { get; set; }
        public string EmailFrom { get; set; }
        public string ResetPasswordUrlTemplate { get; set; }

        public string Signature { get; set; }
    }
}