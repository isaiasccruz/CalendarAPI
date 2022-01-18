using CalendarAPI.Domain.Enums;
using CalendarAPI.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CalendarAPI.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class DapperConnection : IDatabase
    {
        private DbConnection _connection;

        private async Task CreateConnection(DataBase db)
        {
            string connectionString = string.Empty;
            if (db == DataBase.defaultDB)
            {
                string user = Environment.GetEnvironmentVariable("DB_DEF_USR");
                string password = Environment.GetEnvironmentVariable("DB_DEF_PWD");
                connectionString = Environment.GetEnvironmentVariable(db.ToString().ToUpper()).Replace("{user}", user).Replace("{password}", password);

                _connection = new SqlConnection(connectionString);
            }
            else
            {
                connectionString = Environment.GetEnvironmentVariable(db.ToString().ToUpper());
                _connection = new SqlConnection(connectionString);
            }

            await _connection.OpenAsync();
        }

        public async Task Execute(string sql, DataBase db, object param = null)
        {
            await CreateConnection(db);

            using (DbTransaction trans = _connection.BeginTransaction())
            {
                await _connection.ExecuteAsync(sql, param, trans, 60, CommandType.Text);
                trans.Commit();
            }
        }
        public async Task ExecuteWithRetry(string sql, DataBase db, object param = null)
        {
            await CreateConnection(db);

            using (DbTransaction trans = _connection.BeginTransaction())
            {
                await DapperExtensions.ExecuteAsyncWithRetry(_connection, sql, param, trans, 60, CommandType.Text);
                trans.Commit();
            }
        }

        public async Task<IEnumerable<T>> Query<T>(string sql, DataBase db, object param = null)
        {
            await CreateConnection(db);

            try
            {
                return await _connection.QueryAsync<T>(sql, param, null, 60, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<IEnumerable<T>> QueryWithRetry<T>(string sql, DataBase db, object param = null)
        {
            await CreateConnection(db);

            try
            {
                return await DapperExtensions.QueryAsyncWithRetry<T>(_connection, sql, param, null, 60, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<dynamic> Query(string sql, DataBase db, object param = null)
        {
            await CreateConnection(db);

            try
            {
                return await _connection.QueryAsync(sql, param, null, 60, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<int?> Insert<T>(T obj, DataBase db)
        {
            try
            {
                await CreateConnection(db);
                int? id = await _connection.InsertAsync<T>(obj);
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<int> Update<T>(T obj, DataBase db)
        {
            try
            {
                await CreateConnection(db);
                int id = await _connection.UpdateAsync<T>(obj);
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> Delete<T>(object obj, DataBase db)
        {
            try
            {
                await CreateConnection(db);
                int id = await _connection.DeleteAsync<T>(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<T> Get<T>(object obj, DataBase db)
        {
            try
            {
                await CreateConnection(db);
                return await _connection.GetAsync<T>(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<IEnumerable<T>> Get<T>(DataBase db)
        {
            try
            {
                await CreateConnection(db);
                return await _connection.GetListAsync<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
