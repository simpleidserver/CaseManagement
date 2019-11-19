using System.Collections.Generic;

namespace CaseManagement.Workflow.Engine
{
    public class ProcessFlowInstanceExecutionContext
    {
        private Dictionary<string, string> _variables;

        public ProcessFlowInstanceExecutionContext()
        {
            _variables = new Dictionary<string, string>();
        }

        public ProcessFlowInstanceExecutionContext(string callingOperation) : this()
        {
            CallingOperation = callingOperation;
        }

        public string CallingOperation { get; private set; }

        public string GetVariable(string str)
        {
            if (!_variables.ContainsKey(str))
            {
                return null;
            }

            return _variables[str];
        }

        public void SetVariable(string str, string value)
        {
            _variables.Add(str, value);
        }
    }
}
