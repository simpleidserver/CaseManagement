export class HumanTaskDefinitionDeadLines {
    startDeadLines: HumanTaskDefinitionDeadLine[];
    completionDeadLines: HumanTaskDefinitionDeadLine[];
}

export class HumanTaskDefinitionDeadLine {
    name: string;
    for: string;
    until: string;
}