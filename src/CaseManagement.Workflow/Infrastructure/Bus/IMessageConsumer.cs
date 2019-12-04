namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public interface IMessageConsumer
    {
        void Start();
        void Stop();
        string QueueName { get; }
    }
}
