import { CaseFile } from './case-file.model';

export class SearchCaseFilesResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: CaseFile[];

    public static fromJson(json: any): SearchCaseFilesResult
    {
        let result = new SearchCaseFilesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: CaseFile[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(CaseFile.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}