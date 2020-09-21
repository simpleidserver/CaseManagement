namespace CaseManagement.BPMN.Domains
{
    public enum ActivityStates
    {
        READY = 0,
        ACTIVE = 1,
        COMPLETING = 2,
        WITHDRAW = 3,
        COMPLETED = 5,
        FAILING = 6,
        TERMINATING = 7,
        COMPENSATING = 8,
        COMPENSETATED = 9,
        FAILED = 10,
        TERMINATED = 11
    }
}
