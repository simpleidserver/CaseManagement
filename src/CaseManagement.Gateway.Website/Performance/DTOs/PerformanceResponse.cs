using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.Performance.DTOs
{
    [DataContract]
    public class PerformanceResponse
    {
        [DataMember(Name = "datetime")]
        public DateTime DateTime { get; set; }
        [DataMember(Name = "machine_name")]
        public string MachineName { get; set; }
        [DataMember(Name = "nb_working_threads")]
        public int NbWorkingThreads { get; set; }
        [DataMember(Name = "memory_consumed_mb")]
        public double MemoryConsumedMB { get; set; }
    }
}