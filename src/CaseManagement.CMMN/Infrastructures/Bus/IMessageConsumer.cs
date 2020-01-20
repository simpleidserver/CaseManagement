namespace CaseManagement.CMMN.Infrastructures.Bus
{
    public interface IMessageConsumer
    {
        void Start();
        void Stop();
        string QueueName { get; }
    }
}
