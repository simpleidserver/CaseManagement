using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Persistence;
using CaseManagement.Workflow.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace CaseManagement.Workflow
{
    public class WorkflowServerBuilder
    {
        public WorkflowServerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; private set; }

        public WorkflowServerBuilder AddForms(ICollection<Form> forms)
        {
            Services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            Services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            return this;
        }
    }
}