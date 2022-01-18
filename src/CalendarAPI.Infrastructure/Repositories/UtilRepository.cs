using CalendarAPI.Domain.Repositories;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CalendarAPI.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UtilRepository : IUtilRepository
    {
        public IDictionary<string, string> UpsertDictionary(IDictionary<string, string> dic, string _chave, string val)
        {
            if (dic == null) dic = new Dictionary<string, string>();

            if (dic.ContainsKey(_chave))
            {
                dic.Remove(_chave);
            }
            dic.Add(_chave, val);
            return dic;
        }
    }
}
