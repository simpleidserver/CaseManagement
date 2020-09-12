using System;

namespace CaseManagement.CMMN.Infrastructure.Jobs
{
    [Serializable]
    public class BaseNotification
    {
        public BaseNotification(string id)
        {
            Id = id;
            NbRetry = 0;
        }

        public void Increment()
        {
            NbRetry++;
        }

        public string Id { get; private set; }
        public int NbRetry { get; set; }
    }
}
