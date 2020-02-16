export class CaseElementDefinitionHistory {
    Element: string;
    NbInstances: number;

    public static fromJson(json: any): CaseElementDefinitionHistory {
        let result = new CaseElementDefinitionHistory();
        result.Element = json["element"];
        result.NbInstances = json["nb_instances"];
        return result;
    }
}

export class CaseDefinitionHistory {
    constructor() {
        this.Elements = [];
    }

    Id: string;
    NbInstances: number;
    Elements: CaseElementDefinitionHistory[];

    public static fromJson(json: any): CaseDefinitionHistory {
        var result = new CaseDefinitionHistory();
        result.Id = json["id"];
        result.NbInstances = json["nb_instances"];
        json["elements"].forEach(function (sh: any) {
            result.Elements.push(CaseElementDefinitionHistory.fromJson(sh));
        });

        return result;
    }
}