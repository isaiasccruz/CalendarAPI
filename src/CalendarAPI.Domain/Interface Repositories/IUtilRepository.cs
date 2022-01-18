using System.Collections.Generic;

namespace CalendarAPI.Domain.Repositories
{
    public interface IUtilRepository
    {
        IDictionary<string, string> UpsertDictionary(IDictionary<string, string> dic, string _chave, string val);
    }
}
