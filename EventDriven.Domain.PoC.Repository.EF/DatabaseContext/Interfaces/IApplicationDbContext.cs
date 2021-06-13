namespace EventDriven.Domain.PoC.Repository.EF.DatabaseContext.Interfaces
{
    public interface IApplicationDbContext
    {
        ApplicationDbContext UnderlyingContext();
    }
}