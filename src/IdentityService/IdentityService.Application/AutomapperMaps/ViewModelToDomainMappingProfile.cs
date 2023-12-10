using AutoMapper;
using IdentityService.Application.ViewModels.ApplicationRoles;
using IdentityService.Application.ViewModels.ApplicationUsers;
using IdentityService.Application.ViewModels.ApplicationUsers.Response;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RefreshTokenEntity;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

namespace IdentityService.Application.AutomapperMaps;

public class ViewModelToDomainMappingProfile : Profile
{
    public ViewModelToDomainMappingProfile()
    {
        ConfigureMappings();
    }

    public override string ProfileName => "ViewModelToDomainMappingProfile";

    /// <summary>
    ///     Creates a mapping between source (Domain) and destination (ViewModel)
    /// </summary>
    private void ConfigureMappings()
    {
        CreateMap<RoleViewModel, Role>();
        CreateMap<UserViewModel, User>();
        CreateMap<RefreshTokenViewModel, RefreshToken>();
    }
}