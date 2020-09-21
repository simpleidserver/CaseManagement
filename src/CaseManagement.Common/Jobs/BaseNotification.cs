using System;

namespace CaseManagement.Common.Jobs
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
