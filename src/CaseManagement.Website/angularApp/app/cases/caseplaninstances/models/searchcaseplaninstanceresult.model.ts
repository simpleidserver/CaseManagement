import { CasePlanInstance } from './caseplaninstance.model';

export class SearchCasePlanInstanceResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CasePlanInstance[];

    public static fromJson(json: any): SearchCasePlanInstanceResult
    {
        let result = new SearchCasePlanInstanceResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CasePlanInstance[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CasePlanInstance.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}