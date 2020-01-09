using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Persistence.Responses
{
    public class CaseDefinitionResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GroupedWorkflowDefinitionsResult
    {
        public GroupedWorkflowDefinitionsResult()
        {
            CaseDefinitions = new List<CaseDefinitionResult>();
        }

        public string CmmnFileName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public ICollection<CaseDefinitionResult> CaseDefinitions { get; set; }
    }
}
