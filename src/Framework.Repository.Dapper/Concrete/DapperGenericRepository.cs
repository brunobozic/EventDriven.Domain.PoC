using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Framework.Repository.Dapper.Contracts;
using Microsoft.Data.Sqlite;

namespace Framework.Repository.Dapper.Concrete;

public class DapperGenericRepository<TEntity> : IDapperGenericRepository<TEntity> where TEntity : class
{
    private readonly IDbConnection _conn;
    private readonly string _myConnectionString;
    private string _identityField;

    public DapperGenericRepository(
        IDbConnection conn
        , char parameterIdentified = ':'
    )
    {
        _conn = conn ?? throw new ArgumentNullException(nameof(conn),
            $"The parameter {nameof(conn)} can't be null");
        ParameterIdentified = parameterIdentified;
        PartsQryGenerator = new PartsQryGenerator<TEntity>(ParameterIdentified);
        IdentityInspector = new IdentityInspector<TEntity>(conn);
    }

    private IPartsQryGenerator<TEntity> PartsQryGenerator { get; }
    private IIDentityInspector<TEntity> IdentityInspector { get; }

    /// <summary>
    ///     Identifier parameter (@) to SqlServer (:) to Oracle
    /// </summary>
    protected char ParameterIdentified { get; set; }

    private string QrySelect { get; set; }
    private string QryInsert { get; set; }

    public IEnumerable<TEntity> All()
    {
        CreateSelectQry();

        var result = _conn.Query<TEntity>(QrySelect);

        return result;
    }

    public Task<IEnumerable<TEntity>> AllAsync()
    {
        return Task.Run(() => All());
    }

    public IEnumerable<TEntity> GetData(string qry, object parameters)
    {
        ParameterValidator.ValidateString(qry, nameof(qry));
        ParameterValidator.ValidateObject(parameters, nameof(parameters));

        var result = _conn.Query<TEntity>(qry, parameters);

        return result;
    }

    public Task<IEnumerable<TEntity>> GetDataAsync(string qry, object parameters)
    {
        ParameterValidator.ValidateString(qry, nameof(qry));
        ParameterValidator.ValidateObject(parameters, nameof(parameters));

        var result = _conn.QueryAsync<TEntity>(qry, parameters);

        return result;
    }

    public IEnumerable<TEntity> GetData(object filter)
    {
        ParameterValidator.ValidateObject(filter, nameof(filter));

        var selectQry = PartsQryGenerator.GenerateSelect(filter);

        var result = _conn.Query<TEntity>(selectQry, filter);

        return result;
    }

    public Task<IEnumerable<TEntity>> GetDataAsync(object filter)
    {
        return Task.Run(() => GetData(filter));
    }

    public TEntity Find(object pksFields)
    {
        ParameterValidator.ValidateObject(pksFields, nameof(pksFields));

        var selectQry = PartsQryGenerator.GenerateSelect(pksFields);

        var result = _conn.Query<TEntity>(selectQry, pksFields).FirstOrDefault();

        return result;
    }

    public Task<TEntity> FindAsync(object pksFields)
    {
        return Task.Run(() => Find(pksFields));
    }

    public int Add(TEntity entity)
    {
        if (_conn == null)
            throw new ArgumentNullException(nameof(entity), $"The parameter {nameof(entity)} can't be null");

        CreateInsertQry();

        var result = _conn.Execute(QryInsert, entity);

        return result;
    }

    public Task<int> AddAsync(TEntity entity)
    {
        if (_conn == null)
            throw new ArgumentNullException(nameof(entity), $"The parameter {nameof(entity)} can't be null");

        CreateInsertQry();

        var result = _conn.ExecuteAsync(QryInsert, entity);

        return result;
    }

    public int Add(IEnumerable<TEntity> entities)
    {
        ParameterValidator.ValidateEnumerable(entities, nameof(entities));

        CreateInsertQry();

        var result = _conn.Execute(QryInsert, entities);

        return result;
    }

    public Task<int> AddAsync(IEnumerable<TEntity> entities)
    {
        ParameterValidator.ValidateEnumerable(entities, nameof(entities));

        CreateInsertQry();

        var result = _conn.ExecuteAsync(QryInsert, entities);

        return result;
    }

    public void Remove(object key)
    {
        ParameterValidator.ValidateObject(key, nameof(key));

        var deleteQry = PartsQryGenerator.GenerateDelete(key);

        _conn.Execute(deleteQry, key);
    }

    public Task RemoveAsync(object key)
    {
        ParameterValidator.ValidateObject(key, nameof(key));

        var deleteQry = PartsQryGenerator.GenerateDelete(key);

        return _conn.ExecuteAsync(deleteQry, key);
    }

    public int Update(TEntity entity, object pks)
    {
        ParameterValidator.ValidateObject(entity, nameof(entity));
        ParameterValidator.ValidateObject(pks, nameof(pks));

        var updateQry = PartsQryGenerator.GenerateUpdate(pks);

        var result = _conn.Execute(updateQry, entity);

        return result;
    }

    public Task<int> UpdateAsync(TEntity entity, object pks)
    {
        ParameterValidator.ValidateObject(entity, nameof(entity));
        ParameterValidator.ValidateObject(pks, nameof(pks));

        var updateQry = PartsQryGenerator.GenerateUpdate(pks);

        var result = _conn.ExecuteAsync(updateQry, entity);

        return result;
    }

    public int InstertOrUpdate(TEntity entity, object pks)
    {
        ParameterValidator.ValidateObject(entity, nameof(entity));
        ParameterValidator.ValidateObject(pks, nameof(pks));

        int result;

        var entityInTable = Find(pks);

        result = entityInTable == null ? Add(entity) : Update(entity, pks);

        return result;
    }

    public Task<int> InstertOrUpdateAsync(TEntity entity, object pks)
    {
        return Task.Run(() => InstertOrUpdate(entity, pks));
    }

    public void Dispose()
    {
        try
        {
            _conn.Dispose();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private static string CreateConnectionString(string dbPath)
    {
        return new SqliteConnectionStringBuilder { DataSource = dbPath }.ConnectionString;
    }

    private void CreateSelectQry()
    {
        if (string.IsNullOrWhiteSpace(QrySelect))
            QrySelect = PartsQryGenerator.GenerateSelect();
    }

    private void CreateInsertQry()
    {
        if (!string.IsNullOrWhiteSpace(QryInsert)) return;
        _identityField = IdentityInspector.GetColumnsIdentityForType();

        QryInsert = PartsQryGenerator.GeneratePartInsert(_identityField);
    }
}