export class CaseDefinition {
    Id: string;
    Name: string;
    CreateDateTime: Date;
    Xml: string;

    public static fromJson(json: any): CaseDefinition {
        var result = new CaseDefinition();
        result.Id = json["id"];
        result.Name = json["name"];
        result.CreateDateTime = json["create_datetime"];
        result.Xml = json["xml"];
        return result;
    }
}