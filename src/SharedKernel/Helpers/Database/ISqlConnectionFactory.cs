using System.Data;

namespace SharedKernel.Helpers.Database;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}