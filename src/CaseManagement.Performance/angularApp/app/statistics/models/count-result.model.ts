export class CountResult {
    Count: number;

    public static fromJson(json: any): CountResult {
        var result = new CountResult();
        result.Count = json["count"];
        return result;
    }
}