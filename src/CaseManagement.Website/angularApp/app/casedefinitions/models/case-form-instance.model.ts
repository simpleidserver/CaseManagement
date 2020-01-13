import { CaseTranslation } from "./case-translation.model";

export class CaseFormElementInstance {
    constructor() {
        this.Names = [];
        this.Descriptions = [];
    }

    FormElementId: string;
    IsRequired: boolean;
    Value: string;
    Type: string;
    Names: CaseTranslation[];
    Descriptions: CaseTranslation[];

    public static fromJson(json: any): CaseFormElementInstance {
        let result = new CaseFormElementInstance();
        result.FormElementId = json["form_element_id"];
        result.IsRequired = json["is_required"];
        result.Value = json["value"];
        result.Type = json["type"];
        for (var key in json) {
            if (key.startsWith("name")) {
                result.Names.push(CaseTranslation.fromJson(json["key"]));
            }
            else if (key.startsWith("description")) {
                result.Descriptions.push(CaseTranslation.fromJson(json["description"]));
            }
        }

        return result;
    }
}

export class CaseFormInstance {
    constructor() {
        this.Titles = [];
        this.Content = [];
    }

    Id: string;
    CreateDateTime: Date;
    UpdateDateTime: Date;
    Performer: string;
    CaseDefinitionId: string;
    CaseInstanceId: string;
    CaseElementDefinitionId: string;
    CaseElementInstanceId: string;
    Status: string;
    FormId: string;
    Titles: CaseTranslation[];
    Content: CaseFormElementInstance[];

    public static fromJson(json: any): CaseFormInstance {
        var result = new CaseFormInstance();
        result.Id = json["id"];
        result.CreateDateTime = json["create_datetime"];
        result.UpdateDateTime = json["update_datetime"];
        result.Performer = json["performer"];
        result.CaseDefinitionId = json["case_definition_id"];
        result.CaseInstanceId = json["case_instance_id"];
        result.CaseElementDefinitionId = json["case_element_definition_id"];
        result.CaseElementInstanceId = json["case_element_instance_id"];
        result.Status = json["status"];
        result.FormId = json["form_id"];
        for (var key in json) {
            if (key.startsWith("title")) {
                result.Titles.push(CaseTranslation.fromJson(json[key]));
            }
        }

        json["content"].forEach(function (elt : any) {
            result.Content.push(CaseFormElementInstance.fromJson(elt));
        });

        return result;
    }
}