namespace CaseManagement.HumanTask.Domains
{
    public enum HumanTaskInstanceStates
    {
        CREATED = 0,
        READY = 1,
        RESERVED = 2,
        INPROGRESS = 3,
        COMPLETED = 4,
        FAILED = 5,
        ERROR = 6,
        EXITED = 7,
        OBSOLETE = 8
    }
}
