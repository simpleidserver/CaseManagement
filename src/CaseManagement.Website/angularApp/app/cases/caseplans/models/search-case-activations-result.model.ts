import { CaseActivation } from './case-activation.model';

export class SearchCaseActivationsResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CaseActivation[];

    public static fromJson(json: any): SearchCaseActivationsResult
    {
        let result = new SearchCaseActivationsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CaseActivation[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CaseActivation.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}