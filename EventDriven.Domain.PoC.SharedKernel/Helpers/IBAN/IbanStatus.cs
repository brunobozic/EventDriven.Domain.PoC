namespace EventDriven.Domain.PoC.SharedKernel.Helpers.IBAN
{
    public class IbanStatus
    {
        public bool IsValid;
        public string Message;

        public IbanStatus(string message, bool isValid = false)
        {
            IsValid = isValid;
            Message = message;
        }
    }
}