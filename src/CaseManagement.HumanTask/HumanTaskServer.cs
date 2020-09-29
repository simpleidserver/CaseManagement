using CaseManagement.Common.Jobs;
using System.Collections.Generic;

namespace CaseManagement.HumanTask
{
    public class HumanTaskServer : IHumanTaskServer
    {
        private readonly IEnumerable<IJob> _jobs;

        public HumanTaskServer(IEnumerable<IJob> jobs)
        {
            _jobs = jobs;
        }

        public void Start()
        {
            foreach (var messageConsumer in _jobs)
            {
                messageConsumer.Start();
            }
        }

        public void Stop()
        {
            foreach (var messageConsumer in _jobs)
            {
                try
                {
                    messageConsumer.Stop().Wait();
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
