using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains
{
    public class Message : ICloneable
    {
        public Message()
        {
            Items = new List<ItemDefinition>();
        }

        public string Name { get; set; }
        public ICollection<ItemDefinition> Items { get; set; }

        public object Clone()
        {
            return new Message
            {
                Name = Name,
                Items = Items.Select(_ => (ItemDefinition)_.Clone()).ToList()
            };
        }
    }
}
