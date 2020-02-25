export class CountResult {
    NbCaseFiles: number;
    NbCasePlans: number;

    public static fromJson(json: any): CountResult {
        var result = new CountResult();
        result.NbCaseFiles = json["nb_case_files"];
        result.NbCasePlans = json["nb_case_plans"];
        return result;
    }
}