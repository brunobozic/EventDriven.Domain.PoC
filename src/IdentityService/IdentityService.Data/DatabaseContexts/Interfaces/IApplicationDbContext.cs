namespace IdentityService.Data.DatabaseContexts.Interfaces;

public interface IApplicationDbContext
{
    ApplicationDbContext UnderlyingContext();
}