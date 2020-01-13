namespace CaseManagement.CMMN.Domains
{
    public class HumanTask : CMMNTask
    {
        public HumanTask(string name) : base(name)
        {
        }

        public string FormId { get; set; }
        public string PerformerRef { get; set; }

        public override object Clone()
        {
            return new HumanTask(Name)
            {
                IsBlocking = IsBlocking,
                FormId = FormId,
                PerformerRef = PerformerRef
            };
        }
    }
}
