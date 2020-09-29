using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Parser
{
    public interface IOperationParametersParser
    {
        Dictionary<string, string> Parse(ICollection<Parameter> parameters, Dictionary<string, string> entries);
    }
}
