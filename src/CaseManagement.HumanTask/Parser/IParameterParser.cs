using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Parser
{
    public interface IParameterParser
    {
        NotificationInstancePeopleAssignment ParseNotificationInstancePeopleAssignment(NotificationDefinitionPeopleAssignment assignmentDef, Dictionary<string, string> parameters);
        HumanTaskInstancePeopleAssignment ParseHumanTaskInstancePeopleAssignment(HumanTaskDefinitionAssignment assignmentDef, Dictionary<string, string> parameters);
        PeopleAssignmentInstance ParsePeopleAssignment(PeopleAssignmentDefinition assignment, Dictionary<string, string> parameters);
        PresentationElementInstance ParsePresentationElement(PresentationElementDefinition presentationElement, Dictionary<string, string> operationParameters);
        string ParsePresentationContent(string content, Dictionary<string, string> parameters);
        Dictionary<string, string> ParseToPartParameters(ICollection<ToPart> parts, Dictionary<string, string> parameters);
        Dictionary<string, string> ParsePresentationParameters(ICollection<PresentationParameter> parameters, Dictionary<string, string> operationParameters);
        Dictionary<string, string> ParseOperationParameters(ICollection<Parameter> parameters, Dictionary<string, string> entries);
        bool IsValid(string value, ParameterTypes type);
    }
}
