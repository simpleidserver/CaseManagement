import { Role } from './role.model';

export class SearchRolesResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: Role[];

    public static fromJson(json: any): SearchRolesResult
    {
        let result = new SearchRolesResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: Role[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(Role.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}