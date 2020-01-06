using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace CaseManagement.CMMN
{
    public class CMMNServerBuilder
    {
        private IServiceCollection _services;

        public CMMNServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public CMMNServerBuilder AddDefinitions(List<string> pathLst)
        {
            var result = new List<CMMNWorkflowDefinition>();
            foreach(var path in pathLst)
            {
                result.AddRange(CMMNParser.ExtractWorkflowDefinition(path));
            }
            
            _services.AddSingleton<ICMMNWorkflowDefinitionQueryRepository>(new InMemoryCMMNWorkflowDefinitionQueryRepository(result));
            return this;
        }

        public CMMNServerBuilder AddCaseProcesses(ICollection<ProcessAggregate> caseProcesses)
        {
            _services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            return this;
        }

        public CMMNServerBuilder AddForms(ICollection<FormAggregate> forms)
        {
            _services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            _services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            return this;
        }

        /*
        public CMMNServerBuilder AddForms(ICollection<FormAggregate> forms)
        {
            Services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            Services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            return this;
        }

        public CMMNServerBuilder AddRoles(ICollection<RoleAggregate> roles)
        {
            Services.AddSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            Services.AddSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            return this;
        }

        public CMMNServerBuilder AddDefinitions(Action<CMMNDefinitionsBuilder> callback)
        {
            var builder = new CMMNDefinitionsBuilder();
            callback(builder);
            Services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(builder.Build()));
            return this;
        }

        public CMMNServerBuilder AddDefinitions(ICollection<tDefinitions> defs)
        {
            Services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(defs));
            return this;
        }

        public CMMNServerBuilder AddCaseProcesses(ICollection<ProcessAggregate> caseProcesses)
        {
            Services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            return this;
        }
        */
    }
}
