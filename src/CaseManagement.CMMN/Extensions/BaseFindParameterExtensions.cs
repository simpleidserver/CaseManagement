using CaseManagement.Workflow.Persistence.Parameters;
using Microsoft.AspNetCore.Http;

namespace CaseManagement.CMMN.Extensions
{
    public static class BaseFindParameterExtensions
    {
        public static void ExtractFindParameter(this BaseFindParameter parameter, IQueryCollection query)
        {
            int startIndex, count;
            string orderBy;
            FindOrders findOrder;
            if (query.TryGet("start_index", out startIndex))
            {
                parameter.StartIndex = startIndex;
            }

            if (query.TryGet("count", out count))
            {
                parameter.Count = count;
            }

            if (query.TryGet("order_by", out orderBy))
            {
                parameter.OrderBy = orderBy;
            }

            if (query.TryGet("order", out findOrder))
            {
                parameter.Order = findOrder;
            }
        }
    }
}
