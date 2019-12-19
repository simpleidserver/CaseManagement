namespace CaseManagement.CMMN.Domains
{
    public class CMMNHumanTask : CMMNTask
    {
        public CMMNHumanTask(string name) : base(name)
        {
        }

        public string FormId { get; set; }
        public string PerformerRef { get; set; }

        public override object Clone()
        {
            return new CMMNHumanTask(Name)
            {
                IsBlocking = IsBlocking,
                FormId = FormId,
                PerformerRef = PerformerRef
            };
        }
    }
}
