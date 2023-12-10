using AutoMapper;
using IdentityService.Application.ViewModels.ApplicationRoles;
using IdentityService.Application.ViewModels.ApplicationUsers;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using IdentityService.Application.ViewModels.ApplicationUsers.Request;
using IdentityService.Application.ViewModels.ApplicationUsers.Response;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RefreshTokenEntity;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

namespace IdentityService.Application.AutomapperMaps;

public class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile()
    {
        ConfigureMappings();
    }

    public override string ProfileName => "DomainToViewModelMappingProfile";

    /// <summary>
    ///     Creates a mapping between source (Domain) and destination (ViewModel)
    /// </summary>
    private void ConfigureMappings()
    {
        CreateMap<Role, RoleViewModel>();
        CreateMap<User, ApplicationUserResponse>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    //if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                    return true;
                }
            ));

        CreateMap<RefreshToken, RefreshTokenViewModel>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    //// ignore null role
                    //if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                    return true;
                }
            ));

        CreateMap<User, UserViewModel>()
            //.ForAllMembers(x => x.Condition(
            //      (src, dest, prop) =>
            //      {
            //          // ignore null & empty string properties
            //          if (prop == null) return false;
            //          if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

            //          // ignore null role
            //          //if (x.DestinationMember.Name == "Role" && src.Role == null) return false;
            //          if (x.DestinationMember.Name == "RefreshTokens" && src.RefreshTokens == null) return false;

            //          return true;
            //      }
            //  ))
            ;

        CreateMap<User, AuthenticateResponse>();
        CreateMap<RegisterUserRequest, User>();
        CreateMap<CreateUserCommand, User>();
        CreateMap<Role, UserRoleViewModel>();

        CreateMap<UpdateUserCommand, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Role" && src.RoleEnum == null) return false;

                    return true;
                }
            ));
    }
}