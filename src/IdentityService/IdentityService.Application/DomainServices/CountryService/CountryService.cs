using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace IdentityService.Application.DomainServices.CountryService;

public class CountryService : Service<CountryCodebook>, ICountryService
{
    public CountryService(ITrackableRepository<CountryCodebook> repository) : base(repository)
    {
    }
}