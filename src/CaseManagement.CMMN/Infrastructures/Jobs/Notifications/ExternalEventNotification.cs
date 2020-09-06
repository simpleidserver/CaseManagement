namespace CaseManagement.CMMN.Infrastructures.Jobs.Notifications
{
    public class ExternalEventNotification : BaseNotification
    {
        public ExternalEventNotification(string id) : base(id) { }

        public string EvtName { get; set; }
        public string CasePlanInstanceId { get; set; }
        public string CasePlanElementInstanceId { get; set; }
    }
}
