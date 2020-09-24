using System;

namespace CaseManagement.BPMN.Domains
{
    public class ItemDefinition : BaseElement, ICloneable
    {
        public ItemDefinition()
        {
            IsCollection = false;
        }

        /// <summary>
        /// Defines the nature of the item.
        /// </summary>
        public ItemKinds ItemKind { get; set; }
        /// <summary>
        /// Setting this flag to true indicates that the actual data type is a collection.
        /// </summary>
        public bool IsCollection { get; set; }
        /// <summary>
        /// The concrete data structure to be used.
        /// </summary>
        public string StructureRef { get; set; }

        public object Clone()
        {
            return new ItemDefinition
            {
                Id = Id,
                IsCollection = IsCollection,
                ItemKind = ItemKind,
                StructureRef = StructureRef
            };
        }
    }
}
