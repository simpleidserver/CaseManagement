import { CasePlan } from './case-plan.model';

export class SearchCasePlansResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CasePlan[];

    public static fromJson(json: any): SearchCasePlansResult
    {
        let result = new SearchCasePlansResult();
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