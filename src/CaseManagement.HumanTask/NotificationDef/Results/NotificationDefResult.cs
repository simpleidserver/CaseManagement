using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.NotificationDef.Results
{
    public class NotificationDefResult
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public int NbInstances { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int Priority { get; set; }
        public ICollection<ParameterResult> OperationParameters { get; set; }
        public ICollection<PeopleAssignmentDefinitionResult> PeopleAssignments { get; set; }
        public ICollection<PresentationElementDefinitionResult> PresentationElements { get; set; }
        public ICollection<PresentationParameterResult> PresentationParameters { get; set; }

        public static NotificationDefResult ToDto(NotificationDefinitionAggregate notificationDef)
        {
            return new NotificationDefResult
            {

                OperationParameters = notificationDef.OperationParameters.Select(_ => ParameterResult.ToDto(_)).ToList(),
                PeopleAssignments = notificationDef.PeopleAssignments.Select(_ => PeopleAssignmentDefinitionResult.ToDto(_)).ToList(),
                PresentationElements = notificationDef.PresentationElements.Select(_ => PresentationElementDefinitionResult.ToDto(_)).ToList(),
                PresentationParameters = notificationDef.PresentationParameters.Select(_ => PresentationParameterResult.ToDto(_)).ToList()
            };
        }

        public class ParameterResult
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsRequired { get; set; }
            public string Usage { get; set; }

            public static ParameterResult ToDto(Parameter par)
            {
                return new ParameterResult
                {
                    IsRequired = par.IsRequired,
                    Name = par.Name,
                    Type = Enum.GetName(typeof(ParameterTypes), par.Type),
                    Usage = Enum.GetName(typeof(ParameterUsages), par.Usage).ToUpperInvariant()
                };
            }

            public Parameter ToDomain()
            {
                var type = (ParameterTypes)Enum.Parse(typeof(ParameterTypes), Type.ToUpperInvariant());
                var usage = (ParameterUsages)Enum.Parse(typeof(ParameterUsages), Usage.ToUpperInvariant());
                return new Parameter
                {
                    IsRequired = IsRequired,
                    Name = Name,
                    Type = type,
                    Usage = usage
                };
            }
        }

        public class PresentationElementDefinitionResult
        {
            public string Usage { get; set; }
            public string Language { get; set; }
            public string Value { get; set; }
            public string ContentType { get; set; }

            public static PresentationElementDefinitionResult ToDto(PresentationElementDefinition presElt)
            {
                return new PresentationElementDefinitionResult
                {
                    ContentType = presElt.ContentType,
                    Language = presElt.Language,
                    Usage = Enum.GetName(typeof(PresentationElementUsages), presElt.Usage),
                    Value = presElt.Value
                };
            }

            public PresentationElementDefinition ToDomain()
            {
                return new PresentationElementDefinition
                {
                    ContentType = ContentType,
                    Language = Language,
                    Value = Value,
                    Usage = (PresentationElementUsages)Enum.Parse(typeof(PresentationElementUsages), Usage.ToUpperInvariant())
                };
            }
        }

        public class PresentationParameterResult
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Expression { get; set; }

            public static PresentationParameterResult ToDto(PresentationParameter pp)
            {
                return new PresentationParameterResult
                {
                    Name = pp.Name,
                    Type = Enum.GetName(typeof(ParameterTypes), pp.Type),
                    Expression = pp.Expression
                };
            }

            public PresentationParameter ToDomain()
            {
                var parameterType = (ParameterTypes)Enum.Parse(typeof(ParameterTypes), Type.ToUpper());
                return new PresentationParameter
                {
                    Name = Name,
                    Type = parameterType,
                    Expression = Expression
                };
            }
        }

        public class PeopleAssignmentDefinitionResult
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Usage { get; set; }
            public string Value { get; set; }

            public static PeopleAssignmentDefinitionResult ToDto(PeopleAssignmentDefinition peopleAssignment)
            {
                return new PeopleAssignmentDefinitionResult
                {
                    Id = peopleAssignment.Id,
                    Type = Enum.GetName(typeof(PeopleAssignmentTypes), peopleAssignment.Type),
                    Usage = Enum.GetName(typeof(PeopleAssignmentUsages), peopleAssignment.Usage),
                    Value = peopleAssignment.Value
                };
            }

            public PeopleAssignmentDefinition ToDomain()
            {
                if (string.IsNullOrWhiteSpace(Type) || string.IsNullOrWhiteSpace(Usage))
                {
                    return null;
                }

                return new PeopleAssignmentDefinition
                {
                    Usage = (PeopleAssignmentUsages)Enum.Parse(typeof(PeopleAssignmentUsages), Usage.ToUpperInvariant()),
                    Type = (PeopleAssignmentTypes)Enum.Parse(typeof(PeopleAssignmentTypes), Type.ToUpperInvariant()),
                    Value = Value
                };
            }
        }
    }
}
