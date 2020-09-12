using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CMMNExpression : ICloneable
    {
        public CMMNExpression() { }

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

        public object Clone()
        {
            return new CMMNExpression(Language, Body);
        }
    }
}
