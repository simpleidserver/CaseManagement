namespace CaseManagement.CMMN.Domains
{
    public class CMMNProcessTask : CMMNTask
    {
        public CMMNProcessTask(string name) : base(name)
        {
        }

        public string AssemblyQualifiedName { get; set; }

        public override object Clone()
        {
            return new CMMNProcessTask(Name)
            {
                IsBlocking = IsBlocking,
                State = State,
                AssemblyQualifiedName = AssemblyQualifiedName
            };
        }
    }
}
