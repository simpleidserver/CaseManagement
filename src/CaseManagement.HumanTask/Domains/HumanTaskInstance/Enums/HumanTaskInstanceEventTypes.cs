namespace CaseManagement.HumanTask.Domains
{
    public enum HumanTaskInstanceEventTypes
    {
        CREATED = 0,
        CLAIM = 1,
        START = 2,
        STOP = 3,
        RELEASE = 4,
        SUSPEND = 5,
        SUSPENDUNTIL = 6,
        RESUME = 7,
        COMPLETE = 8,
        REMOVE = 9,
        FAIL = 10,
        SETPRIORITY = 11,
        ADDATTACHMENT = 12,
        DELETEATTACHMENT = 13,
        ADDCOMMENT = 14,
        UPDATECOMMENT = 15,
        DELETECOMMENT = 16,
        SKIP = 17,
        FORWARD = 18,
        DELEGATE = 19,
        SETOUTPUT = 20,
        DELETEOUTPUT = 21,
        SETFAULT = 22,
        DELETEFAULT = 23,
        ACTIVATE = 24,
        NOMINATE = 25,
        SETGENERICHUMANROLE = 26,
        EXPIRE = 27,
        ESCALATED = 28,
        CANCEL = 29
    }
}
