using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CaseFile;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CaseManagement.CMMN
{
    public class ServerBuilder
    {
        private IServiceCollection _services;

        public ServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ServerBuilder AddDefinitions(List<string> pathLst)
        {
            var caseFiles = new List<CaseFileDefinitionAggregate>();
            var caseDefinitions = new ConcurrentBag<CaseDefinition>();
            var caseDefinitionHistories = new ConcurrentBag<CaseDefinitionHistoryAggregate>();
            foreach(var path in pathLst)
            {
                var cmmnTxt = File.ReadAllText(path);
                var caseDefinition = CMMNParser.ExtractWorkflowDefinition(path);
                foreach(var cd in caseDefinition)
                {
                    caseDefinitions.Add(cd);
                }

                caseFiles.Add(new CaseFileDefinitionAggregate
                {
                    Payload = cmmnTxt,
                    CreateDateTime = DateTime.UtcNow,
                    Description = caseDefinition.First().CaseFileId,
                    Id = caseDefinition.First().CaseFileId,
                    Name = caseDefinition.First().CaseFileId
                });
            }

            _services.AddSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(caseFiles));
            _services.AddSingleton<ICaseDefinitionCommandRepository>(new InMemoryCaseDefinitionCommandRepository(caseDefinitions, caseDefinitionHistories));
            _services.AddSingleton<ICaseDefinitionQueryRepository>(new InMemoryCaseDefinitionQueryRepository(caseDefinitions, caseDefinitionHistories));
            return this;
        }

        public ServerBuilder AddCaseProcesses(ICollection<ProcessAggregate> caseProcesses)
        {
            _services.AddSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            return this;
        }

        public ServerBuilder AddForms(ICollection<FormAggregate> forms)
        {
            _services.AddSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            _services.AddSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            return this;
        }

        public ServerBuilder AddStatistics(ConcurrentBag<DailyStatisticAggregate> statistics)
        {
            var performanceStatistics = new ConcurrentBag<PerformanceStatisticAggregate>();
            _services.AddSingleton<IStatisticCommandRepository>(new InMemoryStatisticCommandRepository(statistics, performanceStatistics));
            _services.AddSingleton<IStatisticQueryRepository>(new InMemoryStatisticQueryRepository(statistics, performanceStatistics));
            return this;
        }

        /*
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
