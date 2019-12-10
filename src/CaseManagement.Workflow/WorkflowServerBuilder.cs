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

        public WorkflowServerBuilder AddForms(ICollection<FormAggregate> forms)
        {
            Services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            Services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            return this;
        }

        public WorkflowServerBuilder AddRoles(ICollection<RoleAggregate> roles)
        {
            Services.AddSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            Services.AddSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            return this;
        }
    }
}