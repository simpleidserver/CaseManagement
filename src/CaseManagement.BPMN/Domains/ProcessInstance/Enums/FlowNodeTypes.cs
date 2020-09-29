namespace CaseManagement.BPMN.Domains
{
    public enum FlowNodeTypes
    {
        STARTEVENT = 0,
        EMPTYTASK = 1,
        SUBPROCESS = 2,
        SERVICETASK = 3,
        EXCLUSIVEGATEWAY = 4,
        PARALLELGATEWAY = 5,
        INCLUSIVEGATEWAY = 6,
        USERTASK = 7
    }
}
