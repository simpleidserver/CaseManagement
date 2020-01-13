using System;

namespace CaseManagement.CMMN.Domains
{
    public class DailyStatisticAggregate : ICloneable
    {
        public DateTime DateTime { get; set; }
        public int NbActiveCases { get; set; }
        public int NbCompletedCases { get; set; }
        public int NbTerminatedCases { get; set; }
        public int NbFailedCases { get; set; }
        public int NbSuspendedCases { get; set; }
        public int NbClosedCases { get; set; }
        public int NbCreatedForm { get; set; }
        public int NbConfirmedForm { get; set; }
        public int NbCreatedActivation { get; set; }
        public int NbConfirmedActivation { get; set; }

        public void Increment(CMMNTransitions transition)
        {
            switch (transition)
            {
                case CMMNTransitions.Create:
                case CMMNTransitions.Reactivate:
                    NbActiveCases++;
                    break;
                case CMMNTransitions.Complete:
                    NbCompletedCases++;
                    break;
                case CMMNTransitions.Terminate:
                    NbTerminatedCases++;
                    break;
                case CMMNTransitions.Fault:
                    NbFailedCases++;
                    break;
                case CMMNTransitions.Suspend:
                    NbSuspendedCases++;
                    break;
                case CMMNTransitions.Close:
                    NbClosedCases++;
                    break;
            }
        }

        public void IncrementCreatedForm()
        {
            NbCreatedForm++;
        }

        public void IncrementConfirmedForm()
        {
            NbConfirmedForm++;
        }

        public void IncrementCreatedActivation()
        {
            NbCreatedActivation++;
        }

        public void IncrementConfirmedActivation()
        {
            NbConfirmedActivation++;
        }

        public object Clone()
        {
            return new DailyStatisticAggregate
            {
                DateTime = DateTime,
                NbActiveCases = NbActiveCases,
                NbCompletedCases = NbCompletedCases,
                NbTerminatedCases = NbTerminatedCases,
                NbFailedCases = NbFailedCases,
                NbSuspendedCases = NbSuspendedCases,
                NbClosedCases = NbClosedCases,
                NbConfirmedForm = NbConfirmedForm,
                NbCreatedForm = NbCreatedForm,
                NbConfirmedActivation = NbConfirmedActivation,
                NbCreatedActivation = NbCreatedActivation
            };
        }
    }
}
