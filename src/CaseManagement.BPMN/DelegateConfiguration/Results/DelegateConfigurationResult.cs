using CaseManagement.BPMN.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.BPMN.DelegateConfiguration.Results
{
    public class DelegateConfigurationResult
    {
        public string Id { get; set; }
        public string FullQualifiedName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public Dictionary<string, string> DisplayNames { get; set; }
        public Dictionary<string, string> Descriptions { get; set; }
        public Dictionary<string, string> Records { get; set; }

        public static DelegateConfigurationResult ToDto(DelegateConfigurationAggregate configuration)
        {
            return new DelegateConfigurationResult
            {
                Id = configuration.AggregateId,
                FullQualifiedName = configuration.FullQualifiedName,
                CreateDateTime = configuration.CreateDateTime,
                UpdateDateTime = configuration.UpdateDateTime,
                Descriptions = configuration.Descriptions.ToDictionary(d => d.Language, d => d.Value),
                DisplayNames = configuration.DisplayNames.ToDictionary(d => d.Language, d => d.Value),
                Records = configuration.Records.ToDictionary(d => d.Key, d => d.Value)
            };
        }
    }
}
