export class CaseTranslation {
    Language: string;
    Value: string;

    public static fromJson(json: any): CaseTranslation {
        let result = new CaseTranslation();
        let splitted = json.split('#');
        result.Language = splitted[0];
        result.Value = splitted[1];
        return result;
    }
}