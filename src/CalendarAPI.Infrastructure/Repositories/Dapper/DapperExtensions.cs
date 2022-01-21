using Dapper;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Polly;
using Polly.Retry;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CalendarAPI.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DapperExtensions
    {
        private static readonly IEnumerable<TimeSpan> RetryTimes = new[]
        {
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(3)
        };

        private static readonly AsyncRetryPolicy RetryPolicy = Policy
                                                         .Handle<SqlException>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                                                         .Or<TimeoutException>()
                                                         .OrInner<Win32Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                                                         .WaitAndRetryAsync(RetryTimes,
                                                                        (exception, timeSpan, retryCount, context) =>
                                                                        {
                                                                            Log.Warning(
                                                                                exception,
                                                                                "WARNING: Error communicating w/ ReportingDb, retry after: {RetryTimeSpan}. Tentativa: {RetryCount}",
                                                                                timeSpan,
                                                                                retryCount
                                                                            );
                                                                        });

        public static async Task<int> ExecuteAsyncWithRetry(this IDbConnection cnn, string sql, object param = null,
                                                            IDbTransaction transaction = null, int? commandTimeout = null,
                                                            CommandType? commandType = null)
        {
            return await RetryPolicy.ExecuteAsync(async () => await cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType));
        }

        public static async Task<IEnumerable<T>> QueryAsyncWithRetry<T>(this IDbConnection cnn, string sql, object param = null,
                                                                        IDbTransaction transaction = null, int? commandTimeout = null,
                                                                        CommandType? commandType = null)
        {
            return await RetryPolicy.ExecuteAsync(async () => await cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType));
        }
    }
}
