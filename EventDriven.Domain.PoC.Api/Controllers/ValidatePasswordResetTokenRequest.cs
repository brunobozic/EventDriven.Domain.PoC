using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    public class ValidatePasswordResetTokenRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}