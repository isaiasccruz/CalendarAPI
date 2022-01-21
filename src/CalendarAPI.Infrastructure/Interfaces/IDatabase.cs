using CalendarAPI.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalendarAPI.Infrastructure.Interfaces
{
    public interface IDatabase
    {
        Task<IEnumerable<T>> Query<T>(string sql, DataBase db, object param = null);
        Task<IEnumerable<T>> QueryWithRetry<T>(string sql, DataBase db, object param = null);
        Task<dynamic> Query(string sql, DataBase db, object param = null);
        Task Execute(string sql, DataBase db, object param = null);
        Task ExecuteWithRetry(string sql, DataBase db, object param = null);
        Task<int?> Insert<T>(T obj, DataBase db);
        Task<int> Update<T>(T obj, DataBase db);
        Task<bool> Delete<T>(object obj, DataBase db);
        Task<T> Get<T>(object obj, DataBase db);
        Task<IEnumerable<T>> Get<T>(DataBase db);
    }
}
