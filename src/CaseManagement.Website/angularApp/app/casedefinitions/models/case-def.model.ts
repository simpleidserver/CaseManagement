export class CasePlanModel {
    Id: string;
    Name: string;

    public static fromJson(json: any): CasePlanModel {
        var result = new CasePlanModel();
        result.Id = json["id"];
        result.Name = json["name"];
        return result;
    }
}

export class CaseDefinition {
    Id: string;
    Name: string;
    CreateDateTime: Date;
    CasePlanModels: CasePlanModel[] = [];
    Xml: string;

    public static fromJson(json: any): CaseDefinition {
        var result = new CaseDefinition();
        result.Id = json["id"];
        result.Name = json["name"];
        result.CreateDateTime = json["create_datetime"];
        result.Xml = json["xml"];
        let casePlanModels: CasePlanModel[] = [];
        if (json["cases"]) {
            json["cases"].forEach(function (c: any) {
                casePlanModels.push(CasePlanModel.fromJson(c));
            });
        }

        result.CasePlanModels = casePlanModels;
        return result;
    }
}