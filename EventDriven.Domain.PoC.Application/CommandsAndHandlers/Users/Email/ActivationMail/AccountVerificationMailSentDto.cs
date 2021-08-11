using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email.ActivationMail
{
    public class AccountVerificationMailSentDto
    {
        public bool SuccessfullySent;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActivationLink { get; set; }
        public DateTimeOffset? ActivationLinkGenerated { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}