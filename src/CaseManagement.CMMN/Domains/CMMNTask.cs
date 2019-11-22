namespace CaseManagement.CMMN.Domains
{
    public class CMMNTask : CMMNPlanItem
    {
        public CMMNTask(string id, string name) : base(id, name)
        {
        }

        public bool IsBlocking { get; set; }
    }
}
