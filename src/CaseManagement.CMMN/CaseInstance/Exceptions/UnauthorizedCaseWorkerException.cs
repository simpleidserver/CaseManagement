using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class UnauthorizedCaseWorkerException : Exception
    {
        public UnauthorizedCaseWorkerException(string nameIdentifier, string instanceId, string elementId)
        {
            NameIdentifier = nameIdentifier;
            InstanceId = instanceId;
            ElementId = elementId;
        }

        public string NameIdentifier { get; set; }
        public string InstanceId { get; set; }
        public string ElementId { get; set; }
    }
}
