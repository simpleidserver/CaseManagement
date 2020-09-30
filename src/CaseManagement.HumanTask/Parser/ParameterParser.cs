using CaseManagement.Common.Expression;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Dictionary<string, string> ParsePresentationParameters(ICollection<PresentationParameter> parameters, HumanTaskInstanceAggregate humanTaskInstance)
        {
            var result = new Dictionary<string, string>();
            var context = new HumanTaskInstanceExpressionContext(humanTaskInstance);
            var errors = new List<string>();
            foreach (var parameter in parameters)
            {
                if (string.IsNullOrWhiteSpace(parameter.Expression))
                {
                    continue;
                }

                var str = ExpressionParser.GetString(parameter.Expression, context);
                if (!IsValid(str, parameter.Type))
                {
                    errors.Add(string.Format(Global.InvalidParameterType, parameter.Name, parameter.Type.ToString()));
                    continue;
                }

                result.Add(parameter.Name, str);
            }

            if (errors.Any())
            {
                throw new BadOperationExceptions(errors);
            }

            return result;
        }

        public Dictionary<string, string> ParseOperationParameters(ICollection<Parameter> parameters, Dictionary<string, string> entries)
        {
            var result = new Dictionary<string, string>();
            var errors = new List<string>();
            foreach (var parameter in parameters)
            {
                if (parameter.IsRequired && !entries.ContainsKey(parameter.Name))
                {
                    errors.Add(string.Format(Global.MissingParameter, parameter.Name));
                    continue;
                }

                if (entries.ContainsKey(parameter.Name))
                {
                    var value = entries[parameter.Name];
                    if (!IsValid(value, parameter.Type))
                    {
                        errors.Add(string.Format(Global.InvalidParameterType, parameter.Name, parameter.Type.ToString()));
                        continue;
                    }

                    result.Add(parameter.Name, value);
                }
            }

            if (errors.Any())
            {
                throw new BadOperationExceptions(errors);
            }

            return result;
        }

        public bool IsValid(string value, ParameterTypes type)
        {
            if (!MAPPING_PARAMETERTYPE_TO_VALIDATIONCBS[type](value))
            {
                return false;
            }

            return true;
        }
    }
}
