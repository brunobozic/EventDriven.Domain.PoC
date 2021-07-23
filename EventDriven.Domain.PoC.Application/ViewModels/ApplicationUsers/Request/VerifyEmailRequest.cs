﻿using System;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class VerifyEmailRequest
    {
        public string EmailVerificationToken { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
    }
}