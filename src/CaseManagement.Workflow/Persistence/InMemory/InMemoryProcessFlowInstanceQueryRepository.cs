using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence.Parameters;
using CaseManagement.Workflow.Persistence.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Persistence.InMemory
{
    public class InMemoryProcessFlowInstanceQueryRepository : IProcessFlowInstanceQueryRepository
    {
        private readonly ICollection<ProcessFlowInstance> _processFlowInstances;

        public InMemoryProcessFlowInstanceQueryRepository(ICollection<ProcessFlowInstance> processFlowInstances)
        {
            _processFlowInstances = processFlowInstances;
        }

        public Task<ProcessFlowInstance> FindFlowInstanceById(string id)
        {
            return Task.FromResult(_processFlowInstances.FirstOrDefault(p => p.Id == id));
        }

        public Task<FindResponse<ProcessFlowInstance>> Find(FindWorkflowInstanceParameter parameter)
        {
            IQueryable<ProcessFlowInstance> result = _processFlowInstances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameter.ProcessFlowTemplateId))
            {
                result = result.Where(r => r.ProcessFlowTemplateId == parameter.ProcessFlowTemplateId);
            }

            switch (parameter.Order)
            {
                case FindOrders.ASC:
                    if (parameter.OrderBy == "id")
                    {
                        result = result.OrderBy(r => r.Id);
                    }

                    if (parameter.OrderBy == "template_id")
                    {
                        result = result.OrderBy(r => r.ProcessFlowTemplateId);
                    }

                    if (parameter.OrderBy == "name")
                    {
                        result = result.OrderBy(r => r.ProcessFlowName);
                    }

                    if (parameter.OrderBy == "create_datetime")
                    {
                        result = result.OrderBy(r => r.CreateDateTime);
                    }
                    break;
                case FindOrders.DESC:
                    if (parameter.OrderBy == "id")
                    {
                        result = result.OrderByDescending(r => r.Id);
                    }

                    if (parameter.OrderBy == "template_id")
                    {
                        result = result.OrderByDescending(r => r.ProcessFlowTemplateId);
                    }

                    if (parameter.OrderBy == "name")
                    {
                        result = result.OrderByDescending(r => r.ProcessFlowName);
                    }

                    if (parameter.OrderBy == "create_datetime")
                    {
                        result = result.OrderByDescending(r => r.CreateDateTime);
                    }

                    break;
            }

            int totalLength = result.Count();
            result = result.Skip(parameter.StartIndex).Take(parameter.Count);
            return Task.FromResult(new FindResponse<ProcessFlowInstance>
            {
                StartIndex = parameter.StartIndex,
                Count = parameter.Count,
                TotalLength = totalLength,
                Content = (ICollection<ProcessFlowInstance>)result.ToList()
            });
        }
    }
}
