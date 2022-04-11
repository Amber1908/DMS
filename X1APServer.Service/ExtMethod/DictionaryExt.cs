using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.ExtMethod
{
    public static class DictionaryExt
    {
        public static T2 GetOrAdd<T1, T2>(this Dictionary<T1, T2> dict, T1 key) where T2 : new()
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new T2());
            }

            return dict[key];
        }

        public static void AddIfNotExist<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
        }
    }
}
