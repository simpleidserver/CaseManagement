using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Parser
{
    public interface IParameterParser
    {
        ICollection<PeopleAssignmentInstance> ParsePeopleAssignments(ICollection<PeopleAssignmentDefinition> assignments, Dictionary<string, string> parameters);
        ICollection<PresentationElementInstance> ParsePresentationElements(ICollection<PresentationElementDefinition> presentationElements, ICollection<PresentationParameter> presentationParameters, Dictionary<string, string> operationParameters);
        string ParsePresentationContent(string content, Dictionary<string, string> parameters);
        Dictionary<string, string> ParseToPartParameters(ICollection<ToPart> parts, Dictionary<string, string> parameters);
        Dictionary<string, string> ParsePresentationParameters(ICollection<PresentationParameter> parameters, Dictionary<string, string> operationParameters);
        Dictionary<string, string> ParseOperationParameters(ICollection<Parameter> parameters, Dictionary<string, string> entries);
        bool IsValid(string value, ParameterTypes type);
    }
}
