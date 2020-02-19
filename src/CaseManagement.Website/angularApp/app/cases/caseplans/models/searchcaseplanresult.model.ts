import { CasePlan } from './caseplan.model';

export class SearchCasePlanResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CasePlan[];

    public static fromJson(json: any): SearchCasePlanResult
    {
        let result = new SearchCasePlanResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CasePlan[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CasePlan.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}