import { Escalation } from "../../common/escalation.model";

export class HumanTaskDefinitionDeadLine {
    constructor() {
        this.escalations = [];
    }

    id: string;
    name: string;
    for: string;
    until: string;
    escalations: Escalation[];
}

export class HumanTaskDefinitionDeadLines {
    constructor() {
        this.startDeadLines = [];
        this.completionDeadLines = [];
    }

    startDeadLines: HumanTaskDefinitionDeadLine[];
    completionDeadLines: HumanTaskDefinitionDeadLine[];
}