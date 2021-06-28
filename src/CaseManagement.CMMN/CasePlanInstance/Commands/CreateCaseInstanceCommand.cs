using MediatR;
using CaseManagement.CMMN.CasePlanInstance.Results;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Commands
{
    public class CreateCaseInstanceCommand : IRequest<CasePlanInstanceResult>
    {
        public string CasePlanId { get; set; }
        public string NameIdentifier { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}