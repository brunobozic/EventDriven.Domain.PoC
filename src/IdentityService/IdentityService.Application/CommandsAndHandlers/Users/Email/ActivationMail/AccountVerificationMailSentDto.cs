using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email.ActivationMail;

public record AccountVerificationMailSentDto(
    bool SuccessfullySent,
    string FirstName,
    string LastName,
    string Email,
    string ActivationLink,
    DateTimeOffset? ActivationLinkGenerated,
    Guid UserId);