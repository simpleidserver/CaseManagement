using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence.EF.DomainMapping;
using CaseManagement.CMMN.Persistence.EF.Models;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.Common.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class CaseWorkerTaskQueryRepository : ICaseWorkerTaskQueryRepository
    {
        private static Dictionary<string, string> MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "performer", "PerformerRole" },
            { "case_plan_id", "CasePlanId" },
            { "case_plan_instance_id", "CasePlanInstanceId" },
            { "case_plan_element_instance_id", "CasePlanElementInstanceId" },
            { "type", "TaskType" },
            { "status", "Status" },
            { "create_datetime", "CreateDateTime" },
            { "update_datetime", "UpdateDateTime" }
        };
        private readonly CaseManagementDbContext _caseManagementDbContext;

        public CaseWorkerTaskQueryRepository(CaseManagementDbContext caseManagementDbContext)
        {
            _caseManagementDbContext = caseManagementDbContext;
        }

        public async Task<SearchResult<CaseWorkerTaskAggregate>> Find(FindCaseWorkerTasksParameter parameter, CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                IQueryable<CaseWorkerTaskModel> result = _caseManagementDbContext.CaseWorkers;
                if (MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
                {
                    result = result.InvokeOrderBy(MAPPING_ACTIVATIONENAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
                }

                int totalLength = result.Count();
                result = result.Skip(parameter.StartIndex).Take(parameter.Count);
                var content = await result.ToListAsync(token);
                return new SearchResult<CaseWorkerTaskAggregate>
                {
                    StartIndex = parameter.StartIndex,
                    Count = parameter.Count,
                    TotalLength = totalLength,
                    Content = content.Select(_ => _.ToDomain()).ToList()
                };
            }
        }

        public async Task<CaseWorkerTaskAggregate> Get(string id, CancellationToken token)
        {
            using (var lck = await _caseManagementDbContext.Lock())
            {
                var result = await _caseManagementDbContext.CaseWorkers.FirstOrDefaultAsync(_ => _.Id == id, token);
                if (result == null)
                {
                    return null;
                }

                return result.ToDomain();
            }
        }

        private async Task<ICollection<RoleClaimModel>> GetRoles(IEnumerable<KeyValuePair<string, string>> cls, CancellationToken token)
        {
            IQueryable<IGrouping<long?, RoleClaimModel>> claims = _caseManagementDbContext.Claims.GroupBy(_ => _.RoleId);
            Expression condition = null;
            ParameterExpression roleParameter = Expression.Parameter(typeof(IGrouping<long?, RoleClaimModel>), "r");
            ParameterExpression claimParameter = Expression.Parameter(typeof(RoleClaimModel), "c");
            Expression claimNameProperty = Expression.Property(claimParameter, "ClaimName");
            Expression claimValueProperty = Expression.Property(claimParameter, "ClaimValue");
            foreach (var cl in cls)
            {
                var tmpCondition = Expression.AndAlso(Expression.Equal(claimNameProperty, Expression.Constant(cl.Key)), Expression.Equal(claimValueProperty, Expression.Constant(cl.Value)));
                if (condition == null)
                {
                    condition = tmpCondition;
                }
                else
                {
                    condition = Expression.OrElse(condition, tmpCondition);
                }
            }

            var lambdaExpr = Expression.Lambda(condition, claimParameter);
            var allMethod = typeof(Enumerable).GetMethods().Where(m => m.Name == "All" && m.GetParameters().Length == 2).First().MakeGenericMethod(typeof(RoleClaimModel));
            var callAll = Expression.Call(null, allMethod, roleParameter, lambdaExpr);
            var callLambdaExpr = Expression.Lambda(callAll, roleParameter);
            var whereMethod = typeof(Queryable).GetMethods().Where(m => m.Name == "Where" && m.GetParameters().Length == 2).First().MakeGenericMethod(typeof(IGrouping<long?, RoleClaimModel>));
            var callWhere = Expression.Call(null, whereMethod, claims.Expression, callLambdaExpr);
            var queryables = (IQueryable<IGrouping<long?, RoleClaimModel>>)callWhere.Method.Invoke(null, new object[] { claims, callLambdaExpr });
            var result = await queryables.ToListAsync(token);
            return result.SelectMany(_ => _).ToList();
        }

    }
}
