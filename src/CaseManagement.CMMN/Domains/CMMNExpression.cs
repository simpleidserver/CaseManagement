namespace CaseManagement.CMMN.Domains
{
    public class CMMNExpression
    {
        public CMMNExpression(string language)
        {
            Language = language;
        }

        public CMMNExpression(string language, string body) : this(language)
        {
            Body = body;
        }

        public string Language { get; set; }
        public string Body { get; set; }
    }
}
