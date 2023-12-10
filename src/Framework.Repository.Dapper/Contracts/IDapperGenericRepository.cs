using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Repository.Dapper.Contracts;

public interface IDapperGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetData(object filter);

    Task<IEnumerable<TEntity>> GetDataAsync(object filter);
}