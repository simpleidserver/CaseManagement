namespace CaseManagement.CMMN.Domains
{
    public enum CMMNTransitions
    {
        None = 0,
        Close = 1,
        Complete = 2,
        Create = 3,
        Disable = 4,
        Enable = 5,
        Fault = 6,
        ManualStart = 7,
        Occur = 8,
        ParentResume = 9,
        ParentSuspend = 10,
        Reactivate = 11,
        Reenable = 12,
        Resume = 13,
        Start = 14,
        Suspend = 15,
        Terminate = 16,
        ParentTerminate = 17,
        Update = 18,
        Replace = 19,
        AddChild = 20,
        RemoveChild = 21,
        AddReference = 22,
        RemoveReference = 23,
        Delete = 24
    }
}
