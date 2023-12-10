namespace IdentityService.Data.DatabaseContext.Interfaces;

public interface IApplicationDbContext
{
    ApplicationDbContext UnderlyingContext();
}