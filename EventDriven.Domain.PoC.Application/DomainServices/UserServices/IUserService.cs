using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using URF.Core.Abstractions.Services;

namespace EventDriven.Domain.PoC.Application.DomainServices.UserServices
{
    public interface IUserService : IService<User>
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress);
        Task<CreateApplicationUserResponse> CreateAsync(CreateApplicationUserRequest model, User currentlyLoggedUser);
        Task<DeleteUserResponse> DeleteAsync(int id, User currentlyLoggedUser);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest model, string origin);
        Task<ApplicationUserResponse> GetAllAsync();
        Task<ApplicationUserResponse> GetByIdAsync(int id);
        Task<AuthenticateResponse> RefreshTheTokenAsync(string token, string ipAddress);
        Task<RegisterResponse> RegisterAsync(RegisterApplicationUserRequest model, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest model);
        Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress);
        Task<UpdateRequestResponse> UpdateAsync(int id, UpdateApplicationUserRequest model, User currentlyLoggedUser);
        Task<ValidateResetTokenResponse> ValidateResetTokenAsync(ValidateResetTokenRequest model);
        Task<VerifyEmailResponse> VerifyEmailAsync(string modelToken, string origin);
    }
}