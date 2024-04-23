using IdentityService.Application.ViewModels.ApplicationUsers.Response;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.RefreshTokenCommand;

public class RefreshTokenCommand : CommandBase<AuthenticateResponse>
{
    public string token;
    public string ipAddress;
    public Guid userId;
    private string refreshToken;

    public RefreshTokenCommand(string refreshToken, string origin, string ipAddress)
    {
        this.refreshToken = refreshToken;
        this.Origin = origin;
        this.ipAddress = ipAddress;
    }

    public RefreshTokenCommand(string token, Guid userId, string ipAddress)
    {
        this.token = token;
        this.userId = userId;
        this.ipAddress = ipAddress;
    }

    public Guid Id => Guid.NewGuid();

    private string Origin;
}

