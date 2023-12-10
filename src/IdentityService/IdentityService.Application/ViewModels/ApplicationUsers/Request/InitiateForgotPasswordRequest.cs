﻿using System;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public class InitiateForgotPasswordRequest
{
    public string Email { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}