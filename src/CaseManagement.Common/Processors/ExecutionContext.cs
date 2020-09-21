using CaseManagement.Common.Domains;

namespace CaseManagement.Common.Processors
{
    public class ExecutionContext<T> where T : BaseAggregate
    {
        public T Instance { get; set; }
    }
}
