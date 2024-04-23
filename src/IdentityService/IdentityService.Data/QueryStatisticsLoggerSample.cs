using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Data;

public static class QueryStatisticsLoggerSample
{
    public class InfoMessageInterceptor : DbConnectionInterceptor
    {
        #region InfoMessageInterceptor

        public override DbConnection ConnectionCreated(ConnectionCreatedEventData eventData, DbConnection result)
        {
            var logger = eventData.Context!.GetService<ILoggerFactory>().CreateLogger("InfoMessageLogger");
            ((SqlConnection)eventData.Connection).InfoMessage += (_, args) =>
            {
                logger.LogInformation(1, args.Message);
            };
            return result;
        }

        #endregion InfoMessageInterceptor
    }

    public class StatisticsCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            command.CommandText = "SET STATISTICS IO ON;" + Environment.NewLine + command.CommandText;

            return result;
        }

        #region ReaderExecutingAsync

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            command.CommandText = "SET STATISTICS IO ON;" + Environment.NewLine + command.CommandText;

            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        #endregion ReaderExecutingAsync

        public override InterceptionResult DataReaderClosing(
            DbCommand command,
            DataReaderClosingEventData eventData,
            InterceptionResult result)
        {
            eventData.DataReader.NextResult();

            return result;
        }

        #region DataReaderClosingAsync

        public override async ValueTask<InterceptionResult> DataReaderClosingAsync(
            DbCommand command,
            DataReaderClosingEventData eventData,
            InterceptionResult result)
        {
            await eventData.DataReader.NextResultAsync();

            return result;
        }

        #endregion DataReaderClosingAsync
    }
}