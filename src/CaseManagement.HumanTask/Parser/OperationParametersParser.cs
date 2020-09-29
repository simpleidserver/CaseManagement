using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.Exceptions;
using CaseManagement.HumanTask.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Parser
{
    public class OperationParametersParser : IOperationParametersParser
    {
        private Dictionary<ParameterTypes, Func<string, bool>> MAPPING_PARAMETERTYPE_TO_VALIDATIONCBS = new Dictionary<ParameterTypes, Func<string, bool>>
        {
            { ParameterTypes.BOOL, (str) => bool.TryParse(str, out bool r)  },
            { ParameterTypes.DOUBLE, (str) => double.TryParse(str, out double r)  },
            { ParameterTypes.INT, (str) => int.TryParse(str, out int r)  },
            { ParameterTypes.STRING, (str) => !string.IsNullOrEmpty(str)  },
        };

        public Dictionary<string, string> Parse(ICollection<Parameter> parameters, Dictionary<string, string> entries)
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
                    if (!MAPPING_PARAMETERTYPE_TO_VALIDATIONCBS[parameter.Type](value))
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
    }
}
