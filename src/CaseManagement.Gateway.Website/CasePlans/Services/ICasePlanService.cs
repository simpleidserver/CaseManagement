using CaseManagement.Gateway.Website.CasePlans.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.Services
{
    public interface ICasePlanService
    {
        Task<FindResponse<CasePlanResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries);
    }
}
