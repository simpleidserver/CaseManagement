using System;

namespace CaseManagement.HumanTask.Domains
{
    public class CallbackOperation : ICloneable
    {
        public string Id { get; set; }
        public string Url { get; set; }

        public object Clone()
        {
            return new CallbackOperation
            {
                Id = Id,
                Url = Url
            };
        }
    }
}