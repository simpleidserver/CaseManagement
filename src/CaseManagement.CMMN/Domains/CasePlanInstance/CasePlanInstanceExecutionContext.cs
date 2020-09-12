using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanInstanceExecutionContext : ICloneable
    {
        public CasePlanInstanceExecutionContext()
        {
            Variables = new ConcurrentDictionary<string, string>();
        }

        public ConcurrentDictionary<string, string> Variables;

        public string GetVariable(string key)
        {
            if (!Variables.ContainsKey(key))
            {
                return null;
            }

            return Variables[key];
        }

        public bool SetVariable(string key, string value)
        {
            return Variables.TryAdd(key, value);
        }

        public object Clone()
        {
            return new CasePlanInstanceExecutionContext
            {
                Variables = new ConcurrentDictionary<string, string>(Variables.ToDictionary(c => c.Key, c => c.Value))
            };
        }
    }
}