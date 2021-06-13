using System.ComponentModel;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate
{
    public enum RegistrationStatusEnum : byte
    {
        [Description("Verified")] Verified = 1,
        [Description("VerificationFailed")] VerificationFailed = 2,

        [Description("WaitingForVerification")]
        WaitingForVerification = 3,
        [Description("VerificationTimeOut")] VerificationTimeOut = 4
    }
}