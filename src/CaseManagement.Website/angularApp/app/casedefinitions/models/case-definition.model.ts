export class CaseDefinition {
    Id: string;
    Name: string;
    Description: string;
    CaseFile: string;
    CreateDateTime: Date;

    public static fromJson(json: any): CaseDefinition {
        var result = new CaseDefinition();
        result.Id = json["id"];
        result.Name = json["name"];
        result.Description = json["description"];
        result.CaseFile = json["case_file"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    }
}