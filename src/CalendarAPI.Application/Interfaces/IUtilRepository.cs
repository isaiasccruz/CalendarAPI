using System.Collections.Generic;

namespace CalendarAPI.Application.Interfaces
{
    public interface IUtilRepository
    {
        IDictionary<string, string> UpsertDictionary(IDictionary<string, string> dic, string _key, string val);
    }
}
