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

        public virtual ICollection<PeopleAssignmentInstance> ParsePeopleAssignments(ICollection<PeopleAssignmentDefinition> assignments, Dictionary<string, string> parameters)
        {
            var result = new List<PeopleAssignmentInstance>();
            var executionContext = new BaseExpressionContext(parameters);
            foreach (var record in assignments)
            {
                var value = record.Type == PeopleAssignmentTypes.EXPRESSION ? ExpressionParser.GetString(record.Value, executionContext) : record.Value;
                result.Add(new PeopleAssignmentInstance
                {
                    Type = record.Type,
                    Usage = record.Usage,
                    Value = record.Value
                });
            }

            return result;
        }

        public virtual ICollection<PresentationElementInstance> ParsePresentationElements(ICollection<PresentationElementDefinition> presentationElements, ICollection<PresentationParameter> presentationParameters, Dictionary<string, string> operationParameters)
        {
            var result = new List<PresentationElementInstance>();
            var parameters = ParsePresentationParameters(presentationParameters, operationParameters);
            foreach(var presentationElt in presentationElements)
            {
                result.Add(new PresentationElementInstance
                {
                    ContentType = presentationElt.ContentType,
                    Language = presentationElt.Language,
                    Usage = presentationElt.Usage,
                    Value = ParsePresentationContent(presentationElt.Value, parameters)
                });
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
