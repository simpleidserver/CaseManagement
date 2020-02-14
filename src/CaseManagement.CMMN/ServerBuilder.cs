using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Parser;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace CaseManagement.CMMN
{
    public class ServerBuilder
    {
        private IServiceCollection _services;

        public ServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ServerBuilder AddDefinitions(List<string> pathLst, string caseOwner = null)
        {
            var caseFiles = new ConcurrentBag<CaseFileAggregate>();
            var caseDefinitions = new ConcurrentBag<CasePlanAggregate>();
            foreach(var path in pathLst)
            {
                var cmmnTxt = File.ReadAllText(path);
                var name = Path.GetFileName(path);
                var caseFile = CaseFileAggregate.New(name, name, 0, caseOwner, cmmnTxt);
                caseFile.Update(name, name, cmmnTxt, caseOwner);
                var tDefinitions = CMMNParser.ParseWSDL(cmmnTxt);
                var caseDefinition = CMMNParser.ExtractCasePlans(tDefinitions, caseFile);
                foreach(var cd in caseDefinition)
                {
                    caseDefinitions.Add(cd);
                }

                caseFiles.Add(caseFile);
            }

            _services.TryUpdateSingleton<ICaseFileQueryRepository>(new InMemoryCaseFileQueryRepository(caseFiles));
            _services.TryUpdateSingleton<ICaseFileCommandRepository>(new InMemoryCaseFileCommandRepository(caseFiles));
            _services.TryUpdateSingleton<ICasePlanCommandRepository>(new InMemoryCasePlanCommandRepository(caseDefinitions));
            _services.TryUpdateSingleton<ICasePlanQueryRepository>(new InMemoryCasePlanQueryRepository(caseDefinitions));
            return this;
        }

        public ServerBuilder AddCaseProcesses(ICollection<ProcessAggregate> caseProcesses)
        {
            _services.TryUpdateSingleton<IProcessQueryRepository>(new InMemoryProcessQueryRepository(caseProcesses));
            return this;
        }

        public ServerBuilder AddForms(ICollection<FormAggregate> forms)
        {
            _services.TryUpdateSingleton<IFormQueryRepository>(new InMemoryFormQueryRepository(forms));
            _services.TryUpdateSingleton<IFormCommandRepository>(new InMemoryFormCommandRepository(forms));
            return this;
        }

        public ServerBuilder AddStatistics(ConcurrentBag<DailyStatisticAggregate> statistics)
        {
            var performanceStatistics = new ConcurrentBag<PerformanceAggregate>();
            _services.TryUpdateSingleton<IDailyStatisticCommandRepository>(new InMemoryDailyStatisticCommandRepository(statistics));
            _services.TryUpdateSingleton<IStatisticQueryRepository>(new InMemoryDailyStatisticQueryRepository(statistics));
            return this;
        }

        public ServerBuilder AddRoles(ICollection<RoleAggregate> roles)
        {
            _services.TryUpdateSingleton<IRoleCommandRepository>(new InMemoryRoleCommandRepository(roles));
            _services.TryUpdateSingleton<IRoleQueryRepository>(new InMemoryRoleQueryRepository(roles));
            return this;
        }
    }
}
