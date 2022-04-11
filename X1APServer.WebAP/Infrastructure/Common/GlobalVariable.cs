using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Infrastructure.Common
{
    public class GlobalVariable
    {
        private static GlobalVariable instance = new GlobalVariable();

        private Dictionary<string, string> _store { get; set; }

        private GlobalVariable()
        {
            _store = new Dictionary<string, string>();
        }

        public static GlobalVariable Instance
        {
            get
            {
                return instance;
            }
        }

        public bool TryAdd(string key, string value)
        {
            try
            {
                _store.Add(key, value);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string Get(string key)
        {
            string value;
            _store.TryGetValue(key, out value);
            return value;
        }

        public bool ContainsKey(string key)
        {
            return _store.ContainsKey(key);
        }
    }
}