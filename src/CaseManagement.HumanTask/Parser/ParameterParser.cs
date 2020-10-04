using CaseManagement.Common.Expression;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CaseManagement.HumanTask.Parser
{
    public class ParameterParser : IParameterParser
    {
        protected Dictionary<ParameterTypes, Func<string, bool>> MAPPING_PARAMETERTYPE_TO_VALIDATIONCBS = new Dictionary<ParameterTypes, Func<string, bool>>
        {
            { ParameterTypes.BOOL, (str) => bool.TryParse(str, out bool r)  },
            { ParameterTypes.DOUBLE, (str) => double.TryParse(str, out double r)  },
            { ParameterTypes.INT, (str) => int.TryParse(str, out int r)  },
            { ParameterTypes.STRING, (str) => !string.IsNullOrEmpty(str)  },
        };

        public virtual NotificationInstancePeopleAssignment ParseNotificationInstancePeopleAssignment(NotificationDefinitionPeopleAssignment assignmentDef, Dictionary<string, string> parameters)
        {
            var result = new NotificationInstancePeopleAssignment();
            if (assignmentDef.BusinessAdministrator != null)
            {
                result.BusinessAdministrator = ParsePeopleAssignment(assignmentDef.BusinessAdministrator, parameters);
            }

            if (assignmentDef.Recipient != null)
            {
                result.Recipient = ParsePeopleAssignment(assignmentDef.Recipient, parameters);
            }

            return result;
        }

        public virtual HumanTaskInstancePeopleAssignment ParseHumanTaskInstancePeopleAssignment(HumanTaskDefinitionAssignment assignmentDef, Dictionary<string, string> parameters)
        {
            var result = new HumanTaskInstancePeopleAssignment();
            if (assignmentDef.BusinessAdministrator != null)
            {
                result.BusinessAdministrator = ParsePeopleAssignment(assignmentDef.BusinessAdministrator, parameters);
            }

            if (assignmentDef.ExcludedOwner != null)
            {
                result.ExcludedOwner = ParsePeopleAssignment(assignmentDef.ExcludedOwner, parameters);
            }

            if (assignmentDef.PotentialOwner != null)
            {
                result.PotentialOwner = ParsePeopleAssignment(assignmentDef.PotentialOwner, parameters);
            }

            if (assignmentDef.Recipient != null)
            {
                result.Recipient = ParsePeopleAssignment(assignmentDef.Recipient, parameters);
            }

            if (assignmentDef.TaskInitiator != null)
            {
                result.TaskInitiator = ParsePeopleAssignment(assignmentDef.TaskInitiator, parameters);
            }

            if (assignmentDef.TaskStakeHolder != null)
            {
                result.TaskStakeHolder = ParsePeopleAssignment(assignmentDef.TaskStakeHolder, parameters);
            }

            return result;
        }

        public virtual PeopleAssignmentInstance ParsePeopleAssignment(PeopleAssignmentDefinition assignment, Dictionary<string, string> parameters)
        {
            switch (assignment.Type)
            {
                case PeopleAssignmentTypes.GROUPNAMES:
                    var groupNames = assignment as GroupNamesAssignmentDefinition;
                    return PeopleAssignmentInstance.AssignGroupNames(groupNames.GroupNames);
                case PeopleAssignmentTypes.USERIDENTFIERS:
                    var userIdentifiers = assignment as UserIdentifiersAssignmentDefinition;
                    return PeopleAssignmentInstance.AssignUserIdentifiers(userIdentifiers.UserIdentifiers);
                case PeopleAssignmentTypes.LOGICALPEOPLEGROUP:
                    var logicalGroup = assignment as LogicalPeopleGroupAssignmentDefinition;
                    return PeopleAssignmentInstance.AssignLogicalGroup(logicalGroup.LogicalPeopleGroup, logicalGroup.Arguments);
                case PeopleAssignmentTypes.EXPRESSION:
                    var expr = assignment as ExpressionAssignmentDefinition;
                    var executionContext = new BaseExpressionContext(parameters);
                    var userIdentifier = ExpressionParser.GetString(expr.Expression, executionContext);
                    return PeopleAssignmentInstance.AssignUserIdentifiers(new List<string> { userIdentifier });
            }

            return null;
        }

        public virtual PresentationElementInstance ParsePresentationElement(PresentationElementDefinition presentationElement, Dictionary<string, string> operationParameters)
        {
            var result = new PresentationElementInstance();
            var parameters = ParsePresentationParameters(presentationElement.PresentationParameters, operationParameters);
            if (presentationElement.Descriptions != null && presentationElement.Descriptions.Any())
            {
                foreach (var description in presentationElement.Descriptions)
                {
                    description.Value = ParsePresentationContent(description.Value, parameters);
                    result.Descriptions.Add(description);
                }
            }

            return result;
        }

        public virtual string ParsePresentationContent(string content, Dictionary<string, string> parameters)
        {
            var regex = new Regex(@"\$([a-zA-Z]|_)*\$");
            var result = regex.Replace(content, (m) =>
            {
                if (string.IsNullOrWhiteSpace(m.Value))
                {
                    return string.Empty;
                }

                var key = m.Value.TrimStart('$').TrimEnd('$');
                if (!parameters.ContainsKey(key))
                {
                    return string.Empty;
                }

                return parameters[key];
            });

            return result;
        }

        public virtual Dictionary<string, string> ParseToPartParameters(ICollection<ToPart> parts, Dictionary<string, string> parameters)
        {
            var result = new Dictionary<string, string>();
            var context = new BaseExpressionContext(parameters);
            var errors = new List<string>();
            foreach (var parameter in parts)
            {
                if (string.IsNullOrWhiteSpace(parameter.Expression))
                {
                    continue;
                }

                var str = ExpressionParser.GetString(parameter.Expression, context);
                result.Add(parameter.Name, str);
            }

            if (errors.Any())
            {
                throw new BadOperationExceptions(errors);
            }

            return result;
        }

        public virtual Dictionary<string, string> ParsePresentationParameters(ICollection<PresentationParameter> presentationParameters, Dictionary<string, string> parameters)
        {
            var result = new Dictionary<string, string>();
            var context = new BaseExpressionContext(parameters);
            var errors = new List<string>();
            foreach (var presentationParameter in presentationParameters)
            {
                if (string.IsNullOrWhiteSpace(presentationParameter.Expression))
                {
                    continue;
                }

                var str = ExpressionParser.GetString(presentationParameter.Expression, context);
                if (!IsValid(str, presentationParameter.Type))
                {
                    errors.Add(string.Format(Global.InvalidParameterType, presentationParameter.Name, presentationParameter.Type.ToString()));
                    continue;
                }

                result.Add(presentationParameter.Name, str);
            }

            if (errors.Any())
            {
                throw new BadOperationExceptions(errors);
            }

            return result;
        }

        public virtual Dictionary<string, string> ParseOperationParameters(ICollection<Parameter> operationParameters, Dictionary<string, string> parameters)
        {
            var result = new Dictionary<string, string>();
            var errors = new List<string>();
            foreach (var operationParameter in operationParameters)
            {
                if (operationParameter.IsRequired && !parameters.ContainsKey(operationParameter.Name))
                {
                    errors.Add(string.Format(Global.MissingParameter, operationParameter.Name));
                    continue;
                }

                if (parameters.ContainsKey(operationParameter.Name))
                {
                    var value = parameters[operationParameter.Name];
                    if (!IsValid(value, operationParameter.Type))
                    {
                        errors.Add(string.Format(Global.InvalidParameterType, operationParameter.Name, operationParameter.Type.ToString()));
                        continue;
                    }

                    result.Add(operationParameter.Name, value);
                }
            }

            if (errors.Any())
            {
                throw new BadOperationExceptions(errors);
            }

            return result;
        }

        public virtual bool IsValid(string value, ParameterTypes type)
        {
            if (!MAPPING_PARAMETERTYPE_TO_VALIDATIONCBS[type](value))
            {
                return false;
            }

            return true;
        }
    }
}
