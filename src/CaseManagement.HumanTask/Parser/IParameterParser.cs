using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Parser
{
    public interface IParameterParser
    {
        Dictionary<string, string> ParsePresentationParameters(ICollection<PresentationParameter> parameters, HumanTaskInstanceAggregate humanTaskInstance);
        Dictionary<string, string> ParseOperationParameters(ICollection<Parameter> parameters, Dictionary<string, string> entries);
        bool IsValid(string value, ParameterTypes type);
    }
}
