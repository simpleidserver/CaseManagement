using CaseManagement.Workflow.Builders;
using CaseManagement.Workflow.Infrastructure.Bus;
using CaseManagement.Workflow.Infrastructure.Bus.ConsumeDomainEvent;
using CaseManagement.Workflow.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class DomainEventMessageConsumerFixture
    {
        [Fact]
        public async Task When_Queue_Create_ProcessInstance_Domain_Event()
        {
            var instance = ProcessFlowInstanceBuilder.New("templateId", "Case with two tasks")
                .AddCMMNTask("1", "First Task", (c) => { })
                .AddCMMNTask("2", "Second task", (c) => { })
                .AddConnection("1", "2")
                .Build();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCMMN();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var queryRepository = serviceProvider.GetService<IProcessFlowInstanceQueryRepository>();
            var consumers = serviceProvider.GetServices<IMessageConsumer>();
            var queueProvider = serviceProvider.GetService<IQueueProvider>();
            var eventConsumer = consumers.First(c => c.QueueName == DomainEventMessageConsumer.QUEUE_NAME);
            eventConsumer.Start();
            await queueProvider.Queue(instance.DomainEvents.First());
            await Task.Delay(2 * 1000);
            eventConsumer.Stop();

            var existingInstance = await queryRepository.FindFlowInstanceById(instance.Id);
            Assert.NotNull(existingInstance);
        }
    }
}
