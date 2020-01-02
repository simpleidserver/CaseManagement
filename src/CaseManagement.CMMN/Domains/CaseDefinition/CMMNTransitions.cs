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
        Exit = 6,
        Fault = 7,
        ManualStart = 8,
        Occur = 9,
        ParentResume = 10,
        ParentSuspend = 11,
        Reactivate = 12,
        Reenable = 13,
        Resume = 14,
        Start = 15,
        Suspend = 16,
        Terminate = 17,
        ParentTerminate = 18,
        Update = 19,
        Replace = 20,
        AddChild = 21,
        RemoveChild = 22,
        AddReference = 23,
        RemoveReference = 24,
        Delete = 25
    }
}
