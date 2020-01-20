export class CaseFileItem {
    ElementDefinitionId: string;
    ElementInstanceId: string;
    CaseInstanceId: string;
    Value: string;
    Id: string;
    Type: string;
    CreateDateTime: string;

    public static fromJson(json: any): CaseFileItem {
        var result = new CaseFileItem();
        result.ElementDefinitionId = json["element_definition_id"];
        result.ElementInstanceId = json["element_instance_id"];
        result.CaseInstanceId = json["case_instance_id"];
        result.Value = json["value"];
        result.Id = json["id"];
        result.Type = json["type"];
        result.CreateDateTime = json["create_datetime"];
        return result;
    }
}