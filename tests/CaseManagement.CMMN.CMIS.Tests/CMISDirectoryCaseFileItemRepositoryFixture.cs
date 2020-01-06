namespace CaseManagement.CMMN.CMIS.Tests
{
    public class CMISDirectoryCaseFileItemRepositoryFixture
    {
        /*
        [Fact]
        public async Task When_Listen_CMIS_Directory_And_Add_Directory()
        {
            var factory = BuildPlanItemProcessFactory();
            var cmisSessionFactory = new CMISSessionFactory(Options.Create(new CMISOptions()));
            var cmisDirectoryFactory = new CMISDirectoryCaseFileItemRepository(cmisSessionFactory, Options.Create(new CMISOptions()));
            var cancellationTokenSource = new CancellationTokenSource();
            var instance = CMMNWorkflowBuilder.New("templateId", "Case with two tasks")
                .AddCaseFileItem(new CMMNCaseFileItem(Guid.NewGuid().ToString(), "name")
                {
                    Definition = new CMMNCaseFileItemDefinition(CMISDirectoryCaseFileItemRepository.CASE_FILE_ITEM_TYPE)
                })
                .Build();
            var context = new WorkflowHandlerContext(instance, instance.Elements.First(), factory);
            await cmisDirectoryFactory.Start(context, cancellationTokenSource.Token);
        }

        public static ProcessFlowElementProcessorFactory BuildPlanItemProcessFactory(IEnumerable<Type> types = null)
        {
            var serviceCollection = new ServiceCollection();
            var processors = new List<IProcessFlowElementProcessor>
            {
                new CMMNTaskProcessor(new DomainEventWatcher()),
                new CMMNHumanTaskProcessor(new DomainEventWatcher())
            };
            if (types != null)
            {
                var processHandlers = new List<ICaseProcessHandler>
                {
                    new CaseManagementCallbackProcessHandler(serviceCollection.BuildServiceProvider())
                };
                var processAggregates = new List<ProcessAggregate>();
                int i = 1;
                foreach (var type in types)
                {
                    processAggregates.Add(new CaseManagementProcessAggregate
                    {
                        Id = $"PT_{i.ToString()}",
                        AssemblyQualifiedName = type.AssemblyQualifiedName
                    });
                    i++;
                }

                var processQueryRepository = new InMemoryProcessQueryRepository(processAggregates);
                processors.Add(new CMMNProcessTaskProcessor(new CaseLaunchProcessCommandHandler(processQueryRepository, processHandlers), new DomainEventWatcher()));
            }

            return new ProcessFlowElementProcessorFactory(processors);
        }
        */
    }
}
