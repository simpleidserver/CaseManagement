using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.DailyStatistic
{
    public class DailyStatisticService : IDailyStatisticService
    {
        private readonly IStatisticQueryRepository _statisticQueryRepository;

        public DailyStatisticService(IStatisticQueryRepository statisticQueryRepository)
        {
            _statisticQueryRepository = statisticQueryRepository;
        }
        
        public async Task<JObject> Get()
        {
            var currentDateTime = DateTime.UtcNow.Date;
            var result = await _statisticQueryRepository.FindDailyStatistics(new FindDailyStatisticsParameter
            {
                StartDateTime = currentDateTime,
                EndDateTime = currentDateTime
            });
            DailyStatisticAggregate caseDailyStatistic = null;
            if (result.TotalLength == 0)
            {
                caseDailyStatistic = new DailyStatisticAggregate
                {
                    DateTime = currentDateTime
                };
            }
            else
            {
                caseDailyStatistic = result.Content.First();
            }

            return ToDto(caseDailyStatistic);
        }

        public async Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query)
        {
            var result = await _statisticQueryRepository.FindDailyStatistics(ExtractFindParameter(query));
            return ToDto(result);
        }

        private static JObject ToDto(FindResponse<DailyStatisticAggregate> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => ToDto(r))) }
            };
        }

        private static JObject ToDto(DailyStatisticAggregate caseStatistic)
        {
            return new JObject
            {
                { "datetime", caseStatistic.DateTime },
                { "nb_active_cases", caseStatistic.NbActiveCases },
                { "nb_completed_cases", caseStatistic.NbCompletedCases },
                { "nb_terminated_cases", caseStatistic.NbTerminatedCases },
                { "nb_failed_cases", caseStatistic.NbFailedCases },
                { "nb_suspended_cases", caseStatistic.NbSuspendedCases },
                { "nb_closed_cases", caseStatistic.NbClosedCases },
                { "nb_confirmed_forms", caseStatistic.NbConfirmedForm },
                { "nb_created_forms", caseStatistic.NbCreatedForm },
                { "nb_created_activations", caseStatistic.NbCreatedActivation },
                { "nb_confirmed_activations", caseStatistic.NbConfirmedActivation }
            };
        }

        private static FindDailyStatisticsParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            DateTime startDateTime;
            DateTime endDateTime;
            var parameter = new FindDailyStatisticsParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("start_datetime", out startDateTime))
            {
                parameter.StartDateTime = startDateTime;
            }

            if (query.TryGet("end_datetime", out endDateTime))
            {
                parameter.EndDateTime = endDateTime;
            }

            return parameter;
        }
    }
}
