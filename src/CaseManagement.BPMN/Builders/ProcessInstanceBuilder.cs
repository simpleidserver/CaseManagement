using CaseManagement.BPMN.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.BPMN.Builders
{
    public class ProcessInstanceBuilder
    {
        private ProcessInstanceBuilder(string processId, string processFileId)
        {
            ProcessId = processId;
            ProcessFileId = processFileId;
            Builders = new List<FlowNodeBuilder>();
        }

        protected string ProcessId { get; set; }
        protected string ProcessFileId { get; set; }
        protected ICollection<FlowNodeBuilder> Builders;

        public static ProcessInstanceBuilder New(string processId, string processFileId)
        {
            return new ProcessInstanceBuilder(processId, processFileId);
        }

        #region Build events

        public ProcessInstanceBuilder AddStartEvent(string id, string name, Action<StartEventBuilder> callback = null)
        {
            var startEvtBuilder = new StartEventBuilder(id, name);
            if (callback != null)
            {
                callback(startEvtBuilder);
            }

            Builders.Add(startEvtBuilder);
            return this;
        }

        #endregion

        #region Build tasks

        public ProcessInstanceBuilder AddEmptyTask(string id, string name, Action<EmptyTaskBuilder> callback = null)
        {
            var emptyTaskBuilder = new EmptyTaskBuilder(id, name);
            if (callback != null)
            {
                callback(emptyTaskBuilder);
            }

            Builders.Add(emptyTaskBuilder);
            return this;
        }

        #endregion

        public ProcessInstanceAggregate Build()
        {
            var elts = new List<BaseFlowNode>();
            foreach(var builder in Builders)
            {
                elts.Add(builder.Build());
            }

            var result = ProcessInstanceAggregate.New(ProcessFileId, ProcessId, elts);
            return result;
        }
    }
}
