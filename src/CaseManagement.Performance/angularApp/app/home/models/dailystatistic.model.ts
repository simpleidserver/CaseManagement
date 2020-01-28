export class DailyStatistic {
    DateTime: Date;
    NbActiveCases: number;
    NbCompletedCases: number;
    NbTerminatedCases: number;
    NbFailedCases: number;
    NbSuspendedCases: number;
    NbClosedCases: number;
    NbConfirmedForms: number;
    NbCreatedForms: number;
    NbCreatedActivation: number;
    NbConfirmedActivation: number;

    public static fromJson(json: any): DailyStatistic {
        var result = new DailyStatistic();
        result.DateTime = json["datetime"];
        result.NbActiveCases = json["nb_active_cases"];
        result.NbCompletedCases = json["nb_completed_cases"];
        result.NbTerminatedCases = json["nb_terminated_cases"];
        result.NbFailedCases = json["nb_failed_cases"];
        result.NbSuspendedCases = json["nb_suspended_cases"];
        result.NbClosedCases = json["nb_closed_cases"];
        result.NbConfirmedForms = json["nb_confirmed_forms"];
        result.NbCreatedForms = json["nb_created_forms"];
        result.NbCreatedActivation = json["nb_created_activations"];
        result.NbConfirmedActivation = json["nb_confirmed_activations"];
        return result;
    }
}