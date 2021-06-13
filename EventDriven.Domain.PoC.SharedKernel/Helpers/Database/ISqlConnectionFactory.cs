using System.Data;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers.Database
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}