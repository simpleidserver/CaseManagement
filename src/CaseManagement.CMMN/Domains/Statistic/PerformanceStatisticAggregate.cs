using System;

namespace CaseManagement.CMMN.Domains
{
    public class PerformanceStatisticAggregate : ICloneable
    {
        public string MachineName { get; set; }
        public DateTime CaptureDateTime { get; set; }
        public int NbWorkingThreads { get; set; }
        public double MemoryConsumedMB { get; set; }

        public override bool Equals(object obj)
        {
            var target = obj as PerformanceStatisticAggregate;
            if (target == null)
            {
                return false;
            }

            return target.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return MachineName.GetHashCode() + CaptureDateTime.GetHashCode();
        }


        public object Clone()
        {
            return new PerformanceStatisticAggregate
            {
                MachineName = MachineName,
                CaptureDateTime = CaptureDateTime,
                NbWorkingThreads = NbWorkingThreads,
                MemoryConsumedMB = MemoryConsumedMB
            };
        }
    }
}
