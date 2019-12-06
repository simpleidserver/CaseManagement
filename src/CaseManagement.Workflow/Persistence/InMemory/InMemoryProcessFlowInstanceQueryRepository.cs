using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryProcessFlowInstanceQueryRepository : IProcessFlowInstanceQueryRepository
    {
        private static object _obj = new object();
        private static Dictionary<string, string> MAPPING_PROCESSINSTANCE_NAME_TO_PROPERTYNAME = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "template_id", "ProcessFlowTemplateId" },
            { "name", "ProcessFlowName" },
            { "create_datetime", "CreateDateTime" },
            { "status", "Status" },
        };
        private static Dictionary<string, string> MAPPING_PROCESSINSTANCEEXECUTIONSTEP_NAME_TO_PROPERTYNAME = new Dictionary<string,string>
        {
            { "start_datetime", "StartDateTime" },
            { "end_datetime", "EndDateTime" }
        };
        private readonly ICollection<ProcessFlowInstance> _processFlowInstances;

        public InMemoryProcessFlowInstanceQueryRepository(ICollection<ProcessFlowInstance> processFlowInstances)
        {
            _processFlowInstances = processFlowInstances;
        }

        public Task<ProcessFlowInstance> FindFlowInstanceById(string id)
        {
            lock(_obj)
            {
                var result = _processFlowInstances.FirstOrDefault(p => p.Id == id);
                if (result == null)
                {
                    return Task.FromResult<ProcessFlowInstance>(null);
                }

                return Task.FromResult((ProcessFlowInstance)result.Clone());
            }
        }

        public Task<FindResponse<ProcessFlowInstanceExecutionStep>> FindExecutionSteps(FindExecutionStepsParameter parameter)
        {
            var processFlowInstance = _processFlowInstances.FirstOrDefault(p => p.Id == parameter.ProcessFlowInstanceId);
            if (processFlowInstance == null)
            {
                return Task.FromResult(new FindResponse<ProcessFlowInstanceExecutionStep>
                {
                    StartIndex = parameter.StartIndex,
                    Count = parameter.Count,
                    TotalLength = 0,
                    Content = new List<ProcessFlowInstanceExecutionStep>()
                });
            }
        
            IQueryable<ProcessFlowInstanceExecutionStep> result = processFlowInstance.ExecutionSteps.AsQueryable();
            if(MAPPING_PROCESSINSTANCEEXECUTIONSTEP_NAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = InvokeOrderBy(result, MAPPING_PROCESSINSTANCEEXECUTIONSTEP_NAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }
        
            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<ProcessFlowInstanceExecutionStep>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        public Task<FindResponse<ProcessFlowInstance>> Find(FindWorkflowInstanceParameter parameter)
        {
            IQueryable<ProcessFlowInstance> result = _processFlowInstances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.ProcessFlowTemplateId))
            {
                result = result.Where(r => r.ProcessFlowTemplateId == parameter.ProcessFlowTemplateId);
            }

            if (MAPPING_PROCESSINSTANCE_NAME_TO_PROPERTYNAME.ContainsKey(parameter.OrderBy))
            {
                result = InvokeOrderBy(result, MAPPING_PROCESSINSTANCE_NAME_TO_PROPERTYNAME[parameter.OrderBy], parameter.Order);
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<ProcessFlowInstance>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = result.ToList()
            });
        }

        private static IQueryable<T> InvokeOrderBy<T>(IQueryable<T> source, string propertyName, FindOrders order)
        {
            var piParametr = Expression.Parameter(typeof(T), "r");
            var property = Expression.Property(piParametr, propertyName);
            var lambdaExpr = Expression.Lambda(property, piParametr);
            return (IQueryable<T>)Expression.Call(
                typeof(Queryable),
                order == FindOrders.ASC ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), property.Type },
                source.Expression,
                lambdaExpr)
                .Method.Invoke(null, new object[] { source, lambdaExpr });
        }
    }
}
