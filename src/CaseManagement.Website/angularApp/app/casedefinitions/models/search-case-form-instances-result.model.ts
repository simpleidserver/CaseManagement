import { CaseFormInstance } from './case-form-instance.model';

export class SearchCaseFormInstancesResult {
    constructor() {
        this.Content = [];
    }

    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CaseFormInstance[];

    public static fromJson(json: any): SearchCaseFormInstancesResult
    {
        let result = new SearchCaseFormInstancesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CaseFormInstance[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CaseFormInstance.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}