using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.Form.DTOs;
using CaseManagement.Gateway.Website.FormInstance.DTOs;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Results
{
    public class GetCasePlanInstanceResult
    {
        public CasePlanInstanceResponse CasePlanInstance { get; set; }
        public FindResponse<FormInstanceResponse> FormInstances { get; set; }
        public FindResponse<FormResponse> Forms { get; set; }
    }
}
