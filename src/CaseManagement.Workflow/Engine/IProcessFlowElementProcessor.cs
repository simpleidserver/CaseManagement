using CaseManagement.Workflow.Domains;
using System;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public interface IProcessFlowElementProcessor
    {
        Type ProcessFlowElementType { get; }
        Task Handle(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context);
    }
}
