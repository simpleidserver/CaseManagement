namespace CaseManagement.CMMN.Domains
{
    public class CMMNHumanTask : CMMNTask
    {
        public CMMNHumanTask(string name) : base(name)
        {
        }

        public string FormId { get; set; }

        public override object Clone()
        {
            return new CMMNHumanTask(Name)
            {
                IsBlocking = IsBlocking,
                State = State,
                FormId = FormId
            };
        }
    }
}
