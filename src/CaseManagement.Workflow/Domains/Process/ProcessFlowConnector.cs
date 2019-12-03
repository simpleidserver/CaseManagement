using System;

namespace CaseManagement.Workflow.Domains
{
    public class ProcessFlowConnector : ICloneable
    {
        public ProcessFlowConnector(string sourceId, string targetId)
        {
            SourceId = sourceId;
            TargetId = targetId;
        }

        public string SourceId { get; set; }
        public string TargetId { get; set; }

        public object Clone()
        {
            return new ProcessFlowConnector(SourceId, TargetId);
        }
    }
}
