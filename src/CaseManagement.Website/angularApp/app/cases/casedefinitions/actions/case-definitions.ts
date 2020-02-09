import { Action } from '@ngrx/store';
import { CaseDefinition } from '../models/case-definition.model';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';
import { CaseDefinitionHistory } from '../models/case-definition-history.model';

export enum ActionTypes {
    START_SEARCH = "[CaseDefinitions] START_SEARCH",
    COMPLETE_SEARCH = "[CaseDefinitions] COMPLETE_SEARCH",
    START_GET = "[CaseDefinitions] START_GET",
    COMPLETE_GET = "[CaseDefinitions] COMPLETE_GET",
    START_GET_HISTORY = "[CaseDefinitions] START_GET_HISTORY",
    COMPLETE_GET_HISTORY = "[CaseDefinitions] COMPLETE_GET_HISTORY"
}

export class StartFetch implements Action {
    readonly type = ActionTypes.START_SEARCH
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public text: string, public user: string) { }
}

export class CompleteFetch implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH;
    constructor(public content: SearchCaseDefinitionsResult) { }
}

export class StartGet implements Action {
    readonly type = ActionTypes.START_GET
    constructor(public id: string) { }
}

export class CompleteGet implements Action {
    readonly type = ActionTypes.COMPLETE_GET;
    constructor(public content: CaseDefinition) { }
}

export class StartGetHistory implements Action {
    readonly type = ActionTypes.START_GET_HISTORY;
    constructor(public id: string) { }
}

export class CompleteGetHistory implements Action {
    readonly type = ActionTypes.COMPLETE_GET_HISTORY;
    constructor(public content: CaseDefinitionHistory) { }
}

export type ActionsUnion = StartFetch | CompleteFetch | StartGet | CompleteGet | StartGetHistory | CompleteGetHistory;