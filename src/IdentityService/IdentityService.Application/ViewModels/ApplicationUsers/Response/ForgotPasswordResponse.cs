﻿namespace IdentityService.Application.ViewModels.ApplicationUsers.Response;

public class ForgotPasswordResponse
{
    public bool Success { get; internal set; }
    public string Message { get; internal set; }
    public string InnerMessage { get; internal set; }
    public string UserFriendlyMessage { get; internal set; }
}