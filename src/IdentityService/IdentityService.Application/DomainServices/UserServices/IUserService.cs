using System;
using System.Threading.Tasks;
using IdentityService.Application.ViewModels.ApplicationUsers.Request;
using IdentityService.Application.ViewModels.ApplicationUsers.Response;
using IdentityService.Domain.DomainEntities.UserAggregate;
using URF.Core.Abstractions.Services;

namespace IdentityService.Application.DomainServices.UserServices;

public interface IUserService : IService<User>
{
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);

    Task<CreateApplicationUserResponse> CreateAsync(CreateApplicationUserRequest model, User currentlyLoggedUser);

    Task<DeleteUserResponse> DeleteAsync(Guid id, User currentlyLoggedUser);

    // Task<ForgotPasswordResponse> ForgotPasswordAsync(InitiateForgotPasswordRequest model, string origin);
    Task<ApplicationUserResponse> GetAllAsync();

    Task<ApplicationUserResponse> GetByIdAsync(Guid id);

    Task<AuthenticateResponse> RefreshTheTokenAsync(string token, string ipAddress);

    Task<RegisterResponse> RegisterAsync(RegisterUserRequest model, string origin);

    // Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest model);
    Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress);

    Task<UpdateRequestResponse> UpdateAsync(Guid id, UpdateApplicationUserRequest model, User currentlyLoggedUser);

    Task<ValidateResetTokenResponse> ValidateResetTokenAsync(ValidateResetTokenRequest model);

    Task<VerifyEmailResponse> VerifyEmailAsync(string modelToken, string origin);
}