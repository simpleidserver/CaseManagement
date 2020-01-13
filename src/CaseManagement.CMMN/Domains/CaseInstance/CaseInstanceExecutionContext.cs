using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    public class CaseInstanceExecutionContext : ICloneable
    {
        private Dictionary<string, string> _variables;

        public CaseInstanceExecutionContext()
        {
            _variables = new Dictionary<string, string>();
        }

        public CaseInstanceExecutionContext(Dictionary<string, string> variables)
        {
            _variables = variables;
        }

        internal bool ContainsVariable(string str)
        {
            return _variables.ContainsKey(str);
        }

        internal string GetVariable(string str)
        {
            if (!_variables.ContainsKey(str))
            {
                return null;
            }

            return _variables[str];
        }

        internal void SetVariable(string str, string value)
        {
            if (_variables.ContainsKey(str))
            {
                _variables[str] = value;
                return;
            }

            _variables.Add(str, value);
        }

        public object Clone()
        {
            return new CaseInstanceExecutionContext(_variables.ToDictionary(c => c.Key, c => c.Value));
        }

        public Dictionary<string, string> Variables => _variables;
    }
}
