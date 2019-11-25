using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN
{
    public class CMMNServerBuilder
    {
        private readonly IServiceCollection _services;

        public CMMNServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public CMMNServerBuilder AddDefinitions(Action<CMMNDefinitionsBuilder> callback)
        {
            var builder = new CMMNDefinitionsBuilder();
            callback(builder);
            _services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(builder.Build()));
            return this;
        }

        public CMMNServerBuilder AddDefinitions(ICollection<tDefinitions> defs)
        {
            _services.AddSingleton<ICMMNDefinitionsQueryRepository>(new InMemoryCMMNDefinitionsQueryRepository(defs));
            return this;
        }
    }
}
