using CaseManagement.Gateway.Website.Performance.DTOs;
using CaseManagement.Gateway.Website.Performance.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Performance.QueryHandlers
{
    public interface IGetPerformanceQueryHandler
    {
        Task<IEnumerable<string>> Handle(GetPerformanceQuery getPerformanceQuery);
    }
}
