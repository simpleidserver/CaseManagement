using CaseManagement.Gateway.Website.Statistic.Queries;
using CaseManagement.Gateway.Website.Statistic.Results;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.QueryHandlers
{
    public interface ICountQueryHandler
    {
        Task<CountQueryResult> Handle(CountQuery query);
    }
}
