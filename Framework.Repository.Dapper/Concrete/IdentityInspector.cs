using System;
using System.Data;
using System.Linq;
using Dapper;
using Framework.Repository.Dapper.Contracts;

namespace Framework.Repository.Dapper.Concrete
{
    public class IdentityInspector<TEntity> : IIDentityInspector<TEntity> where TEntity : class
    {
        private readonly IDbConnection _conn;

        public IdentityInspector(IDbConnection conn)
        {
            _conn = conn;
        }

        public string GetColumnsIdentityForType()
        {
            string result = null;

            var qry = "select COLUMN_NAME " +
                      "from INFORMATION_SCHEMA.COLUMNS " +
                      "where COLUMNPROPERTY(object_id(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 " +
                      "and TABLE_NAME = @TableName " +
                      "order by TABLE_NAME";

            try
            {
                var tableName = typeof(TEntity).Name;

                result = _conn.Query<string>(qry, new {TableName = tableName}).FirstOrDefault();
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
    }
}