using System;

namespace CaseManagement.HumanTask.Domains
{
    public class PresentationElementInstance : ICloneable
    {
        public long Id { get; set; }
        public PresentationElementUsages Usage { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public string ContentType { get; set; }

        public object Clone()
        {
            return new PresentationElementInstance
            {
                Id = Id,
                Usage = Usage,
                Language = Language,
                Value = Value,
                ContentType = ContentType
            };
        }
    }
}
