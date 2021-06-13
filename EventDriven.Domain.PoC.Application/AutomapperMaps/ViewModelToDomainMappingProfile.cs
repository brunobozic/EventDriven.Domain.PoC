using AutoMapper;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationRoles;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RefreshToken;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

namespace EventDriven.Domain.PoC.Application.AutomapperMaps
{
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
}