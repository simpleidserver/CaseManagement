using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.HumanTaskDef.Results
{
    public class HumanTaskDefResult
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public int NbInstances { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool ActualOwnerRequired { get; set; }
        public int Priority { get; set; }
        public string Outcome { get; set; }
        public string SearchBy { get; set; }
        public ICollection<ParameterResult> OperationParameters { get; set; }
        public ICollection<PeopleAssignmentDefinitionResult> PeopleAssignments { get; set; }
        public ICollection<PresentationElementDefinitionResult> PresentationElements { get; set; }
        public ICollection<RenderingElementResult> RenderingElements { get; set; }
        public ICollection<HumanTaskDefinitionDeadLineResult> DeadLines { get; set; }
        public ICollection<PresentationParameterResult> PresentationParameters { get; set; }
        public ICollection<CallbackOperationResult> CallbackOperations { get; set; }

        public static HumanTaskDefResult ToDto(HumanTaskDefinitionAggregate humanTaskDef)
        {
            return new HumanTaskDefResult
            {
                ActualOwnerRequired = humanTaskDef.ActualOwnerRequired,
                Id = humanTaskDef.AggregateId,
                UpdateDateTime = humanTaskDef.UpdateDateTime,
                CreateDateTime = humanTaskDef.CreateDateTime,
                Name = humanTaskDef.Name,
                NbInstances = humanTaskDef.NbInstances,
                Priority = humanTaskDef.Priority,
                Version = humanTaskDef.Version,
                SearchBy = humanTaskDef.SearchBy,
                Outcome = humanTaskDef.Outcome,
                OperationParameters = humanTaskDef.OperationParameters.Select(_ => ParameterResult.ToDto(_)).ToList(),
                PeopleAssignments = humanTaskDef.PeopleAssignments.Select(_ => PeopleAssignmentDefinitionResult.ToDto(_)).ToList(),
                PresentationElements = humanTaskDef.PresentationElements.Select(_ => PresentationElementDefinitionResult.ToDto(_)).ToList(),
                RenderingElements = humanTaskDef.RenderingElements.Select(_ => RenderingElementResult.ToDto(_)).ToList(),
                DeadLines = humanTaskDef.DeadLines.Select(_ => HumanTaskDefinitionDeadLineResult.ToDto(_)).ToList(),
                PresentationParameters = humanTaskDef.PresentationParameters.Select(_ => PresentationParameterResult.ToDto(_)).ToList()
            };
        }

        public class CallbackOperationResult
        {
            public string Id { get; set; }
            public string Url { get; set; }

            public static CallbackOperationResult ToDto(CallbackOperation op)
            {
                return new CallbackOperationResult
                {
                    Url = op.Url
                };
            }

            public CallbackOperation ToDomain()
            {
                return new CallbackOperation
                {
                    Id = Id,
                    Url = Url
                };
            }
        }

        public class HumanTaskDefinitionDeadLineResult
        {
            public HumanTaskDefinitionDeadLineResult()
            {
                Escalations = new List<EscalationResult>();
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string For { get; set; }
            public string Until { get; set; }
            public string Usage { get; set; }
            public ICollection<EscalationResult> Escalations { get; set; }

            public static HumanTaskDefinitionDeadLineResult ToDto(HumanTaskDefinitionDeadLine humanTaskDefinitionDeadLine)
            {
                return new HumanTaskDefinitionDeadLineResult
                {
                    For = humanTaskDefinitionDeadLine.For,
                    Id = humanTaskDefinitionDeadLine.Id,
                    Name = humanTaskDefinitionDeadLine.Name,
                    Until = humanTaskDefinitionDeadLine.Until,
                    Usage = Enum.GetName(typeof(DeadlineUsages), humanTaskDefinitionDeadLine.Usage).ToUpperInvariant(),
                    Escalations = humanTaskDefinitionDeadLine.Escalations.Select(_ => EscalationResult.ToDto(_)).ToList()
                };
            }

            public HumanTaskDefinitionDeadLine ToDomain()
            {
                return new HumanTaskDefinitionDeadLine
                {
                    Name = Name,
                    For = For,
                    Until = Until
                };
            }
        }

        public class EscalationResult
        {
            public EscalationResult()
            {
                ToParts = new List<ToPartResult>();
            }

            public string Id { get; set; }
            public string Condition { get; set; }
            public ICollection<ToPartResult> ToParts { get; set; }
            public NotificationDefResult Notification { get; set; }
            
            public static EscalationResult ToDto(Escalation esc)
            {
                return new EscalationResult
                {
                    Id = esc.Id,
                    Condition = esc.Condition,
                    ToParts = esc.ToParts.Select(_ => ToPartResult.ToDto(_)).ToList(),
                    Notification = esc.Notification == null ? null : NotificationDefResult.ToDto(esc.Notification)
                };
            }

            public Escalation ToDomain()
            {
                return new Escalation
                {
                    Id = Id,
                    Condition = Condition,
                    Notification = Notification?.ToDomain(),
                    ToParts = ToParts.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public class ToPartResult
        {
            public string Name { get; set; }
            public string Expression { get; set; }

            public static ToPartResult ToDto(ToPart p)
            {
                return new ToPartResult
                {
                    Expression = p.Expression,
                    Name = p.Name
                };
            }

            public ToPart ToDomain()
            {
                return new ToPart
                {
                    Name = Name,
                    Expression = Expression
                };
            }
        }

        public class NotificationDefResult
        {
            public string Name { get; set; }
            public int Priority { get; set; }
            public string Rendering { get; set; }
            public ICollection<ParameterResult> OperationParameters { get; set; }
            public ICollection<PeopleAssignmentDefinitionResult> PeopleAssignments { get; set; }
            public ICollection<PresentationElementDefinitionResult> PresentationElements { get; set; }
            public ICollection<PresentationParameterResult> PresentationParameters { get; set; }

            public static NotificationDefResult ToDto(NotificationDefinition notif)
            {
                return new NotificationDefResult
                {
                    Name = notif.Name,
                    OperationParameters = notif.OperationParameters.Select(_ => ParameterResult.ToDto(_)).ToList(),
                    PeopleAssignments = notif.PeopleAssignments.Select(_ => PeopleAssignmentDefinitionResult.ToDto(_)).ToList(),
                    PresentationElements = notif.PresentationElements.Select(_ => PresentationElementDefinitionResult.ToDto(_)).ToList(),
                    PresentationParameters = notif.PresentationParameters.Select(_ => PresentationParameterResult.ToDto(_)).ToList(),
                    Priority = notif.Priority,
                    Rendering = notif.Rendering
                };
            }

            public NotificationDefinition ToDomain()
            {
                return new NotificationDefinition
                {
                    Name = Name,
                    Priority = Priority,
                    Rendering = Rendering,
                    OperationParameters = OperationParameters.Select(_ => _.ToDomain()).ToList(),
                    PeopleAssignments = PeopleAssignments.Select(_ => _.ToDomain()).ToList(),
                    PresentationElements = PresentationElements.Select(_ => _.ToDomain()).ToList(),
                    PresentationParameters = PresentationParameters.Select(_ => _.ToDomain()).ToList()
                };
            }
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

        public class RenderingElementResult
        {
            public RenderingElementResult()
            {
                Labels = new List<TranslationResult>();
                Values = new List<OptionValueResult>();
            }

            public string Id { get; set; }
            public string XPath { get; set; }
            public string ValueType { get; set; }
            public string Default { get; set; }
            public IEnumerable<OptionValueResult> Values { get; set; }
            public IEnumerable<TranslationResult> Labels { get; set; }

            public static RenderingElementResult ToDto(RenderingElement rd)
            {
                return new RenderingElementResult
                {
                    Id = rd.Id,
                    Default = rd.Default,
                    ValueType = rd.ValueType,
                    XPath = rd.XPath,
                    Values = rd.Values?.Select(_ => OptionValueResult.ToDto(_)),
                    Labels = rd.Labels?.Select(_ => TranslationResult.ToDto(_))
                };
            }

            public RenderingElement ToDomain()
            {
                return new RenderingElement
                {
                    Id = Id,
                    XPath = XPath,
                    ValueType = ValueType,
                    Default = Default,
                    Values = Values?.Select(_ => _.ToDomain()).ToList(),
                    Labels = Labels?.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public class TranslationResult
        {
            public string Language { get; set; }
            public string Value { get; set; }

            public static TranslationResult ToDto(Translation translation)
            {
                return new TranslationResult
                {
                    Language = translation.Language,
                    Value = translation.Value
                };
            }

            public Translation ToDomain()
            {
                return new Translation
                {
                    Language = Language,
                    Value = Value
                };
            }
        }

        public class OptionValueResult
        {
            public OptionValueResult()
            {
                DisplayNames = new List<TranslationResult>();
            }

            public string Value { get; set; }
            public ICollection<TranslationResult> DisplayNames { get; set; }

            public static OptionValueResult ToDto(OptionValue ov)
            {
                return new OptionValueResult
                {
                    Value = ov.Value,
                    DisplayNames = ov.DisplayNames.Select(_ => TranslationResult.ToDto(_)).ToList()
                };
            }

            public OptionValue ToDomain()
            {
                return new OptionValue
                {
                    Value = Value,
                    DisplayNames = DisplayNames.Select(_ => _.ToDomain()).ToList()
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
