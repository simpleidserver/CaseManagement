using System;
using System.Runtime.Serialization;

namespace CaseManagement.Gateway.Website.Statistic.DTOs
{
    [DataContract]
    public class DailyStatisticResponse
    {
        [DataMember(Name = "datetime")]
        public DateTime DateTime { get; set; }
        [DataMember(Name = "nb_active_cases")]
        public int NbActiveCases { get; set; }
        [DataMember(Name = "nb_completed_cases")]
        public int NbCompletedCases { get; set; }
        [DataMember(Name = "nb_terminated_cases")]
        public int NbTerminatedCases { get; set; }
        [DataMember(Name = "nb_failed_cases")]
        public int NbFailedCases { get; set; }
        [DataMember(Name = "nb_suspended_cases")]
        public int NbSuspendedCases { get; set; }
        [DataMember(Name = "nb_closed_cases")]
        public int NbClosedCases { get; set; }
        [DataMember(Name = "nb_confirmed_forms")]
        public int NbConfirmedForms { get; set; }
        [DataMember(Name = "nb_created_forms")]
        public int NbCreatedForms { get; set; }
    }
}