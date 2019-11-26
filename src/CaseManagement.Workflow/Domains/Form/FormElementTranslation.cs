namespace CaseManagement.Workflow.Domains
{
    public class FormElementTranslation
    {
        public FormElementTranslation(string language, string value)
        {
            Language = language;
            Value = value;
        }

        public string Language { get; set; }
        public string Value { get; set; }
    }
}
