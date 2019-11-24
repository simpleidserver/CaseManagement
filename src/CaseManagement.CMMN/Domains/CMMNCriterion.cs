namespace CaseManagement.CMMN.Domains
{
    public class CMMNCriterion
    {
        public CMMNCriterion(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public CMMNSEntry SEntry { get; set; }
    }
}
