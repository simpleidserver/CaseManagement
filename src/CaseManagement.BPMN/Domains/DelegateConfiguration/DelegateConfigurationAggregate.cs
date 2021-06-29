using CaseManagement.Common.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.Domains.DelegateConfiguration
{
    public class DelegateConfigurationAggregate : BaseAggregate
    {
        private const string DESCRIPTION = "description";
        private const string DISPLAYNAME = "displayName";

        public DelegateConfigurationAggregate()
        {
            Translations = new List<BPMNTranslation>();
            Records = new List<DelegateConfigurationRecord>();
        }

        #region Properties

        public string FullQualifiedName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public IEnumerable<BPMNTranslation> DisplayNames
        {
            get
            {
                return Translations.Where(t => t.Type == DISPLAYNAME);
            }
        }
        public IEnumerable<BPMNTranslation> Descriptions
        {
            get
            {
                return Translations.Where(t => t.Type == DESCRIPTION);
            }
        }
        public virtual ICollection<BPMNTranslation> Translations { get; set; }
        public virtual ICollection<DelegateConfigurationRecord> Records { get; set; }

        #endregion

        public override object Clone()
        {
            return new DelegateConfigurationAggregate
            {
                AggregateId = AggregateId,
                Version = Version,
                Translations = Translations.Select(t => (BPMNTranslation)t.Clone()).ToList(),
                Records = Records.Select(r => (DelegateConfigurationRecord)r.Clone()).ToList()
            };
        }

        #region Actions

        public void AddDisplayName(string language, string value)
        {
            Translations.Add(BPMNTranslation.Create($"delegateconf_{DISPLAYNAME}_{language}", value, language, DISPLAYNAME));
        }

        public void AddDescription(string language, string value)
        {
            Translations.Add(BPMNTranslation.Create($"delegateconf_{DESCRIPTION}_{language}", value, language, DESCRIPTION));
        }

        #endregion

        public override void Handle(dynamic evt)
        {
        }
    }
}
