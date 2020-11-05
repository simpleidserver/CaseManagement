using System;

namespace CaseManagement.HumanTask.Domains
{
    public enum PresentationElementUsages
    {
        NAME = 0,
        SUBJECT = 1,
        DESCRIPTION = 2
    }

    public class PresentationElementDefinition : ICloneable
    {
        public long Id { get; set; }
        public PresentationElementUsages Usage { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
        public string ContentType { get; set; }

        public object Clone()
        {
            return new PresentationElementDefinition
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
