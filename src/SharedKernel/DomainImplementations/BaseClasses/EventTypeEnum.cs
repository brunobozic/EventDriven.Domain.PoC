namespace SharedKernel.DomainImplementations.BaseClasses;

public enum EventTypeEnum
{
    Undefined,
    UserCreatedDomainEvent,
    VerificationEmailSend,
    VerificationEmailAcknowledged,
    UserAccountVerified,
    VerificationEmailResent,
    VerificationEmailSendFailure,
    RoleAssignedToUser
}