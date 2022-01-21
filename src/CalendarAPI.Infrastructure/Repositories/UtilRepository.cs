using CalendarAPI.Application.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UtilRepository : IUtilRepository
    {
        public IDictionary<string, string> UpsertDictionary(IDictionary<string, string> dic, string _key, string val)
        {
            if (dic == null) dic = new Dictionary<string, string>();

            if (dic.ContainsKey(_key))
            {
                dic.Remove(_key);
            }
            dic.Add(_key, val);
            return dic;
        }
    }
}
