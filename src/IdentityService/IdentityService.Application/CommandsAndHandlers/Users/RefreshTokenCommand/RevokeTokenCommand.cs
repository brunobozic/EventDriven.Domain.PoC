using IdentityService.Application.ViewModels.ApplicationUsers.Response;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.RefreshTokenCommand;
public class RevokeTokenCommand : CommandBase<RevokeTokenResponse>
{

    private Guid userId;
    private string origin;
    private string ipAddress;
    private string token;

    public RevokeTokenCommand(string token, Guid userId, string origin, string ipAdress)
    {

        this.userId = userId;
        this.origin = origin;
        this.ipAddress = ipAdress;
        this.token = token;
    }

    public Guid Id => Guid.NewGuid();
}

