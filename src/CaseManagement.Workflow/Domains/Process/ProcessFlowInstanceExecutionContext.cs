using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowInstanceExecutionContext : ICloneable
    {
        private Dictionary<string, string> _variables;

        public ProcessFlowInstanceExecutionContext()
        {
            _variables = new Dictionary<string, string>();
        }

        public ProcessFlowInstanceExecutionContext(Dictionary<string, string> variables)
        {
            _variables = variables;
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
            _variables.Add(str, value);
        }

        public object Clone()
        {
            return new ProcessFlowInstanceExecutionContext(_variables.ToDictionary(c => c.Key, c => c.Value));
        }
    }
}
