import { CaseDefinition } from './case-definition.model';

export class SearchCaseDefinitionsResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CaseDefinition[];

    public static fromJson(json: any): SearchCaseDefinitionsResult
    {
        let result = new SearchCaseDefinitionsResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CaseDefinition[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CaseDefinition.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}