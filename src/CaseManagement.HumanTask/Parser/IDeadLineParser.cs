using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Parser
{
    public interface IDeadlineParser
    {
        ICollection<HumanTaskInstanceDeadLine> Evaluate(ICollection<HumanTaskDefinitionDeadLine> deadLines, HumanTaskInstanceDeadLineTypes type, Dictionary<string, string> parameters);
        DateTime? Evaluate(HumanTaskDefinitionDeadLine deadLine, BaseExpressionContext expressionContext);
    }
}