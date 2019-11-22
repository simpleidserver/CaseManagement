using CaseManagement.Workflow.Domains;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Engine
{
    public class ProcessFlowInstanceExecutionContext
    {
        private ProcessFlowInstance _processFlowInstance;
        private Dictionary<string, string> _variables;

        public ProcessFlowInstanceExecutionContext(ProcessFlowInstance processFlowInstance)
        {
            _processFlowInstance = processFlowInstance;
            _variables = new Dictionary<string, string>();
        }

        public ProcessFlowInstanceExecutionContext(ProcessFlowInstance processFlowInstance, string callingOperation) : this(processFlowInstance)
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
        
        public ProcessFlowInstanceElement GetElement(string id)
        {
            return _processFlowInstance.Elements.FirstOrDefault(e => e.Id == id);
        }
    }
}
