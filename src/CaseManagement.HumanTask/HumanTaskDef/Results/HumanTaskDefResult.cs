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
        public DateTime UpdateDateTime { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool ActualOwnerRequired { get; set; }
        public OperationResult Operation { get; set; }
        public int Priority { get; set; }
        public HumanTaskDefAssignmentResult PeopleAssignment { get; set; }
        public PresentationElementDefinitionResult PresentationElementResult { get; set; }
        public string Outcome { get; set; }
        public string SearchBy { get; set; }
        public RenderingResult Rendering { get; set; }
        public HumanTaskDefinitionDeadLinesResult DeadLines { get; set; }

        public static HumanTaskDefResult ToDto(HumanTaskDefinitionAggregate humanTaskDef)
        {
            return new HumanTaskDefResult
            {
                ActualOwnerRequired = humanTaskDef.ActualOwnerRequired,
                Id = humanTaskDef.AggregateId,
                UpdateDateTime = humanTaskDef.UpdateDateTime,
                CreateDateTime = humanTaskDef.CreateDateTime,
                Name = humanTaskDef.Name,
                Priority = humanTaskDef.Priority,
                Version = humanTaskDef.Version,
                SearchBy = humanTaskDef.SearchBy,
                Outcome = humanTaskDef.Outcome,
                Operation = humanTaskDef.Operation == null ? null : OperationResult.ToDto(humanTaskDef.Operation),
                PeopleAssignment = humanTaskDef.PeopleAssignment == null ? null : HumanTaskDefAssignmentResult.ToDto(humanTaskDef.PeopleAssignment),
                PresentationElementResult = humanTaskDef.PresentationElement == null ? null : PresentationElementDefinitionResult.ToDto(humanTaskDef.PresentationElement),
                Rendering = humanTaskDef.Rendering == null ? null : RenderingResult.ToDto(humanTaskDef.Rendering),
                DeadLines = humanTaskDef.DeadLines == null ? null : HumanTaskDefinitionDeadLinesResult.ToDto(humanTaskDef.DeadLines)
            };
        }

        public class HumanTaskDefinitionDeadLinesResult
        {
            public HumanTaskDefinitionDeadLinesResult()
            {
                StartDeadLines = new List<HumanTaskDefinitionDeadLineResult>();
                CompletionDeadLines = new List<HumanTaskDefinitionDeadLineResult>();
            }

            public ICollection<HumanTaskDefinitionDeadLineResult> StartDeadLines { get; set; }
            public ICollection<HumanTaskDefinitionDeadLineResult> CompletionDeadLines { get; set; }

            public static HumanTaskDefinitionDeadLinesResult ToDto(HumanTaskDefinitionDeadLines humanTaskDefinitionDeadLines)
            {
                return new HumanTaskDefinitionDeadLinesResult
                {
                    StartDeadLines = humanTaskDefinitionDeadLines.StartDeadLines.Select(_ => HumanTaskDefinitionDeadLineResult.ToDto(_)).ToList(),
                    CompletionDeadLines = humanTaskDefinitionDeadLines.CompletionDeadLines.Select(_ => HumanTaskDefinitionDeadLineResult.ToDto(_)).ToList()
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
            public ICollection<EscalationResult> Escalations { get; set; }

            public static HumanTaskDefinitionDeadLineResult ToDto(HumanTaskDefinitionDeadLine humanTaskDefinitionDeadLine)
            {
                return new HumanTaskDefinitionDeadLineResult
                {
                    For = humanTaskDefinitionDeadLine.For,
                    Id = humanTaskDefinitionDeadLine.Id,
                    Name = humanTaskDefinitionDeadLine.Name,
                    Until = humanTaskDefinitionDeadLine.Until,
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
            public OperationResult Operation { get; set; }
            public int Priority { get; set; }
            public NotificationDefinitionPeopleAssignmentResult PeopleAssignment { get; set; }
            public PresentationElementDefinitionResult PresentationElement { get; set; }

            public static NotificationDefResult ToDto(NotificationDefinition notif)
            {
                return new NotificationDefResult
                {
                    Name = notif.Name,
                    Operation = notif.Operation == null ? null : OperationResult.ToDto(notif.Operation),
                    PeopleAssignment = notif.PeopleAssignment == null ? null : NotificationDefinitionPeopleAssignmentResult.ToDto(notif.PeopleAssignment),
                    PresentationElement = notif.PresentationElement == null ? null : PresentationElementDefinitionResult.ToDto(notif.PresentationElement),
                    Priority = notif.Priority
                };
            }

            public NotificationDefinition ToDomain()
            {
                return new NotificationDefinition
                {
                    Name = Name,
                    Priority = Priority,
                    Operation = Operation?.ToDomain(),
                    PeopleAssignment = PeopleAssignment?.ToDomain(),
                    PresentationElement = PresentationElement?.ToDomain()
                };
            }
        }

        public class NotificationDefinitionPeopleAssignmentResult
        {
            public PeopleAssignmentResult Recipient { get; set; }
            public PeopleAssignmentResult BusinessAdministrator { get; set; }

            public static NotificationDefinitionPeopleAssignmentResult ToDto(NotificationDefinitionPeopleAssignment notif)
            {
                return new NotificationDefinitionPeopleAssignmentResult
                {
                    BusinessAdministrator = notif.BusinessAdministrator == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(notif.BusinessAdministrator),
                    Recipient = notif.Recipient == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(notif.Recipient)
                };
            }

            public NotificationDefinitionPeopleAssignment ToDomain()
            {
                return new NotificationDefinitionPeopleAssignment
                {
                    Recipient = Recipient?.ToDomain(),
                    BusinessAdministrator = BusinessAdministrator?.ToDomain()
                };
            }
        }

        public class OperationResult
        {
            public OperationResult()
            {
                InputParameters = new List<ParameterResult>();
                OutputParameters = new List<ParameterResult>();
            }

            public ICollection<ParameterResult> InputParameters { get; set; }
            public ICollection<ParameterResult> OutputParameters { get; set; }

            public static OperationResult ToDto(Operation operation)
            {
                return new OperationResult
                {
                    InputParameters = operation.InputParameters.Select(_ => ParameterResult.ToDto(_)).ToList(),
                    OutputParameters = operation.OutputParameters.Select(_ => ParameterResult.ToDto(_)).ToList()
                };
            }

            public Operation ToDomain()
            {
                return new Operation
                {
                    InputParameters = InputParameters.Select(_ => _.ToDomain()).ToList(),
                    OutputParameters = OutputParameters.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public class ParameterResult
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsRequired { get; set; }

            public static ParameterResult ToDto(Parameter par)
            {
                return new ParameterResult
                {
                    IsRequired = par.IsRequired,
                    Name = par.Name,
                    Type = Enum.GetName(typeof(ParameterTypes), par.Type)
                };
            }

            public Parameter ToDomain()
            {
                var type = (ParameterTypes)Enum.Parse(typeof(ParameterTypes), Type.ToUpperInvariant());
                return new Parameter
                {
                    IsRequired = IsRequired,
                    Name = Name,
                    Type = type
                };
            }
        }

        public class PresentationElementDefinitionResult
        {
            public PresentationElementDefinitionResult()
            {
                Names = new List<TextDefResult>();
                Subjects = new List<TextDefResult>();
                Descriptions = new List<DescriptionResult>();
                PresentationParameters = new List<PresentationParameterResult>();
            }

            public ICollection<TextDefResult> Names { get; set; }
            public ICollection<TextDefResult> Subjects { get; set; }
            public ICollection<DescriptionResult> Descriptions { get; set; }
            public ICollection<PresentationParameterResult> PresentationParameters { get; set; }

            public static PresentationElementDefinitionResult ToDto(PresentationElementDefinition presElt)
            {
                return new PresentationElementDefinitionResult
                {
                    Names = presElt.Names.Select(_ => TextDefResult.ToDto(_)).ToList(),
                    Subjects = presElt.Subjects.Select(_ => TextDefResult.ToDto(_)).ToList(),
                    Descriptions = presElt.Descriptions.Select(_ => DescriptionResult.ToDto(_)).ToList(),
                    PresentationParameters = presElt.PresentationParameters.Select(_ => PresentationParameterResult.ToDto(_)).ToList(),
                };
            }

            public PresentationElementDefinition ToDomain()
            {
                return new PresentationElementDefinition
                {
                    Names = Names.Select(_ => _.ToDomain()).ToList(),
                    Subjects = Subjects.Select(_ => _.ToDomain()).ToList(),
                    Descriptions = Descriptions.Select(_ => _.ToDomain()).ToList(),
                    PresentationParameters = PresentationParameters.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public class RenderingResult
        {
            public RenderingResult()
            {
                Input = new List<InputRenderingElementResult>();
                Output = new List<OutputRenderingElementResult>();
            }

            public ICollection<InputRenderingElementResult> Input { get; set; }
            public ICollection<OutputRenderingElementResult> Output { get; set; }

            public static RenderingResult ToDto(Rendering rd)
            {
                return new RenderingResult
                {
                    Input = rd.Input.Select(_ => InputRenderingElementResult.ToDto(_)).ToList(),
                    Output = rd.Output.Select(_ => OutputRenderingElementResult.ToDto(_)).ToList()
                };
            }

            public Rendering ToDomain()
            {
                return new Rendering
                {
                    Input = Input.Select(_ => _.ToDomain()).ToList(),
                    Output = Output.Select(_ => _.ToDomain()).ToList()
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

        public class RenderingElementResult
        {
            public RenderingElementResult()
            {
                Label = new List<TranslationResult>();
            }

            public string Id { get; set; }
            public ICollection<TranslationResult> Label { get; set; }
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

        public class InputRenderingElementResult : RenderingElementResult
        {
            public string Value { get; set; }

            public static InputRenderingElementResult ToDto(InputRenderingElement inputRendering)
            {
                return new InputRenderingElementResult
                {
                    Id = inputRendering.Id,
                    Label = inputRendering.Labels.Select(_ => TranslationResult.ToDto(_)).ToList(),
                    Value = inputRendering.Value
                };
            }

            public InputRenderingElement ToDomain()
            {
                return new InputRenderingElement
                {
                    Id = Id,
                    Labels = Label.Select(_ => _.ToDomain()).ToList(),
                    Value = Value
                };
            }
        }

        public class OutputRenderingElementResult : RenderingElementResult
        {
            public string XPath { get; set; }
            public OutputRenderingElementValueResult Value { get; set; }
            public string Default { get; set; }

            public static OutputRenderingElementResult ToDto(OutputRenderingElement outputRendering)
            {
                return new OutputRenderingElementResult
                {
                    Id = outputRendering.Id,
                    Label = outputRendering.Labels.Select(_ => TranslationResult.ToDto(_)).ToList(),
                    XPath = outputRendering.XPath,
                    Value = outputRendering.Value == null ? null : OutputRenderingElementValueResult.ToDto(outputRendering.Value),
                    Default = outputRendering.Default
                };
            }

            public OutputRenderingElement ToDomain()
            {
                return new OutputRenderingElement
                {
                    Id =  Id,
                    Default = Default,
                    XPath = XPath,
                    Value = Value?.ToDomain(),
                    Labels = Label.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public class OutputRenderingElementValueResult
        {
            public OutputRenderingElementValueResult()
            {
                Values = new List<OptionValueResult>();
            }

            public string Type { get; set; }
            public ICollection<OptionValueResult> Values { get; set; }

            public static OutputRenderingElementValueResult ToDto(OutputRenderingElementValue rv)
            {
                return new OutputRenderingElementValueResult
                {
                    Type = rv.Type,
                    Values = rv.Values.Select(_ => OptionValueResult.ToDto(_)).ToList()
                };
            }

            public OutputRenderingElementValue ToDomain()
            {
                return new OutputRenderingElementValue
                {
                    Type = Type,
                    Values = Values.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public class TextDefResult
        {
            public string Language { get; set; }
            public string Value { get; set; }

            public static TextDefResult ToDto(Text txt)
            {
                return new TextDefResult
                {
                    Language = txt.Language,
                    Value = txt.Value
                };
            }

            public Text ToDomain()
            {
                return new Text
                {
                    Language = Language,
                    Value = Value
                };
            }
        }

        public class DescriptionResult : TextDefResult
        {
            public string ContentType { get; set; }

            public static DescriptionResult ToDto(Description desc)
            {
                return new DescriptionResult
                {
                    Language = desc.Language,
                    Value = desc.Value,
                    ContentType = desc.ContentType
                };
            }

            public new Description ToDomain()
            {
                return new Description
                {
                    Language = Language,
                    Value = Value,
                    ContentType = ContentType
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

        public class HumanTaskDefAssignmentResult
        {
            public PeopleAssignmentResult PotentialOwner { get; set; }
            public PeopleAssignmentResult ExcludedOwner { get; set; }
            public PeopleAssignmentResult TaskInitiator { get; set; }
            public PeopleAssignmentResult TaskStakeHolder { get; set; }
            public PeopleAssignmentResult BusinessAdministrator { get; set; }
            public PeopleAssignmentResult Recipient { get; set; }

            public static HumanTaskDefAssignmentResult ToDto(HumanTaskDefinitionAssignment ass)
            {
                return new HumanTaskDefAssignmentResult
                {
                    PotentialOwner = ass.PotentialOwner == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(ass.PotentialOwner),
                    ExcludedOwner = ass.ExcludedOwner == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(ass.ExcludedOwner),
                    TaskInitiator = ass.TaskInitiator == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(ass.TaskInitiator),
                    TaskStakeHolder = ass.TaskStakeHolder == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(ass.TaskStakeHolder),
                    BusinessAdministrator = ass.BusinessAdministrator == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(ass.BusinessAdministrator),
                    Recipient = ass.Recipient == null ? new PeopleAssignmentResult() : PeopleAssignmentResult.ToDto(ass.Recipient)
                };
            }
        }

        public class PeopleAssignmentResult
        {
            public PeopleAssignmentResult()
            {
                GroupNames = new List<string>();
                UserIdentifiers = new List<string>();
            }

            public string Type { get; set; }
            public string Expression { get; set; }
            public ICollection<string> GroupNames { get; set; }
            public ICollection<string> UserIdentifiers { get; set; }
            public string LogicalPeopleGroup { get; set; }

            public static PeopleAssignmentResult ToDto(PeopleAssignmentDefinition peopleAssignment)
            {
                var type = Enum.GetName(typeof(PeopleAssignmentTypes), peopleAssignment.Type);
                var expr = peopleAssignment as ExpressionAssignmentDefinition;
                if (expr != null)
                {
                    return new PeopleAssignmentResult
                    {
                        Type = type,
                        Expression = expr.Expression
                    };
                }

                var userIds = peopleAssignment as UserIdentifiersAssignmentDefinition;
                if (userIds != null)
                {
                    return new PeopleAssignmentResult
                    {
                        Type = type,
                        UserIdentifiers = userIds.UserIdentifiers
                    };
                }

                var groupNames = peopleAssignment as GroupNamesAssignmentDefinition;
                if (groupNames != null)
                {
                    return new PeopleAssignmentResult
                    {
                        Type = type,
                        GroupNames = groupNames.GroupNames
                    };
                }

                var logicalGroup = peopleAssignment as LogicalPeopleGroupAssignmentDefinition;
                if (logicalGroup != null)
                {
                    return new PeopleAssignmentResult
                    {
                        Type = type,
                        LogicalPeopleGroup = logicalGroup.LogicalPeopleGroup
                    };
                }

                return null;
            }

            public PeopleAssignmentDefinition ToDomain()
            {
                if (string.IsNullOrWhiteSpace(Type))
                {
                    return null;
                }

                var typeEnum = (PeopleAssignmentTypes)Enum.Parse(typeof(PeopleAssignmentTypes), Type.ToUpperInvariant());
                switch(typeEnum)
                {
                    case PeopleAssignmentTypes.EXPRESSION:
                        return new ExpressionAssignmentDefinition
                        {
                            Expression = Expression
                        };
                    case PeopleAssignmentTypes.GROUPNAMES:
                        return new GroupNamesAssignmentDefinition
                        {
                            GroupNames = GroupNames
                        };
                    case PeopleAssignmentTypes.USERIDENTIFIERS:
                        return new UserIdentifiersAssignmentDefinition
                        {
                            UserIdentifiers = UserIdentifiers
                        };
                    case PeopleAssignmentTypes.LOGICALPEOPLEGROUP:
                        return new LogicalPeopleGroupAssignmentDefinition
                        {
                            LogicalPeopleGroup = LogicalPeopleGroup
                        };
                }

                return null;
            }
        }
    }
}
