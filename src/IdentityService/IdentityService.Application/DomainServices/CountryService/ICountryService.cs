using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using URF.Core.Abstractions.Services;

namespace IdentityService.Application.DomainServices.CountryService;

public interface ICountryService : IService<CountryCodebook>
{
    // Add any custom business logic (methods) here
    // All methods in Service<TEntity> are ovverridable for any custom implementations
}