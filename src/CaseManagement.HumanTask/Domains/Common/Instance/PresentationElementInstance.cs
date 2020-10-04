using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class PresentationElementInstance : ICloneable
    {
        public PresentationElementInstance()
        {
            Names = new List<Text>();
            Subjects = new List<Text>();
            Descriptions = new List<Description>();
        }

        /// <summary>
        /// This element is the short title of a task.
        /// </summary>
        public ICollection<Text> Names { get; set; }
        /// <summary>
        /// This element is a longer text that describes the task.
        /// </summary>
        public ICollection<Text> Subjects { get; set; }
        /// <summary>
        /// This element is a long description of the task.
        /// </summary>
        public ICollection<Description> Descriptions { get; set; }

        public object Clone()
        {
            return new PresentationElementInstance
            {
                Names = Names.Select(_ => (Text)_.Clone()).ToList(),
                Subjects = Subjects.Select(_ => (Text)_.Clone()).ToList(),
                Descriptions = Descriptions.Select(_ => (Description)_.Clone()).ToList()
            };
        }
    }
}
