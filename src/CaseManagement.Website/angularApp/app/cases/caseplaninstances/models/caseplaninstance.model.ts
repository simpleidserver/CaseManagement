export class StateHistory {
    State: string;
    DateTime: string;

    public static fromJson(json: any): StateHistory{
        let result = new StateHistory();
        result.State = json["state"];
        result.DateTime = json["datetime"];
        return result;
    }
}

export class TransitionHistory {
    Transition: string;
    DateTime: string;

    public static fromJson(json: any): TransitionHistory {
        let result = new TransitionHistory();
        result.Transition = json["transition"];
        result.DateTime = json["datetime"];
        return result;
    }
}

export class CasePlanElementInstance {
    constructor() {
        this.StateHistories = [];
        this.TransitionHistories = [];
    }

    Id: string;
    Version: string;
    CreateDateTime: Date;
    DefinitionId: string;
    State: string;
    StateHistories: StateHistory[];
    TransitionHistories: TransitionHistory[];

    public static fromJson(json: any): CasePlanElementInstance {
        let result = new CasePlanElementInstance();
        result.Id = json["id"];
        result.Version = json["version"];
        result.CreateDateTime = json["create_datetime"];
        result.DefinitionId = json["definition_id"];
        result.State = json["state"];
        json["state_histories"].forEach(function (sh: any) {
            result.StateHistories.push(StateHistory.fromJson(sh));
        });
        json["transition_histories"].forEach(function (th: any) {
            result.TransitionHistories.push(TransitionHistory.fromJson(th));
        });

        return result;
    }
}

export class CasePlanInstance {
    constructor() {
        this.StateHistories = [];
        this.TransitionHistories = [];
        this.Elements = [];
    }

    Id: string;
    CreateDateTime: Date;
    CasePlanId: string;
    Context: any;
    State: string;
    StateHistories: StateHistory[];
    TransitionHistories: TransitionHistory[];
    Elements: CasePlanElementInstance[];

    public static fromJson(json: any): CasePlanInstance {
        var result = new CasePlanInstance();
        result.Id = json["id"];
        result.CreateDateTime = json["create_datetime"];
        result.CasePlanId = json["case_plan_id"];
        result.Context = json["context"];
        result.State = json["state"];
        json["state_histories"].forEach(function (sh: any) {
            result.StateHistories.push(StateHistory.fromJson(sh));
        });
        json["transition_histories"].forEach(function (th: any) {
            result.TransitionHistories.push(TransitionHistory.fromJson(th));
        });
        json["elements"].forEach(function (elt: any) {
            result.Elements.push(CasePlanElementInstance.fromJson(elt));
        });
        return result;
    }
}