using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class PeopleAssignmentInstance : ICloneable
    {
        public PeopleAssignmentInstance()
        {
            Values = new List<string>();
        }

        public static PeopleAssignmentInstance AssignGroupNames(ICollection<string> groupNames)
        {
            return new PeopleAssignmentInstance
            {
                Values = groupNames,
                Type = PeopleAssignmentTypes.GROUPNAMES
            };
        }

        public static PeopleAssignmentInstance AssignUserIdentifiers(ICollection<string> userIdentifiers)
        {
            return new PeopleAssignmentInstance
            {
                Values = userIdentifiers,
                Type = PeopleAssignmentTypes.USERIDENTFIERS
            };
        }

        public static PeopleAssignmentInstance AssignLogicalGroup(string name, Dictionary<string, string> parameters)
        {
            return new PeopleAssignmentInstance
            {
                LogicalGroup = new LogicalGroupInstance
                {
                    Name = name,
                    Parameters = parameters
                },
                Type = PeopleAssignmentTypes.LOGICALPEOPLEGROUP
            };
        }

        public PeopleAssignmentTypes Type { get; set; }
        public ICollection<string> Values { get; set; }
        public LogicalGroupInstance LogicalGroup { get; set; }

        public object Clone()
        {
            return new PeopleAssignmentInstance
            {
                Type = Type,
                Values = Values.ToList(),
                LogicalGroup = (LogicalGroupInstance)LogicalGroup?.Clone()
            };
        }
    }

    public class LogicalGroupInstance : ICloneable
    {
        public LogicalGroupInstance()
        {
            Parameters = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        public object Clone()
        {
            return new LogicalGroupInstance
            {
                Name = Name,
                Parameters = Parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };
        }
    }
}
