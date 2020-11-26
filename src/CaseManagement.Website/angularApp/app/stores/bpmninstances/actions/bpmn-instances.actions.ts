import { Action } from '@ngrx/store';
import { BpmnInstance } from '../models/bpmn-instance.model';
import { SearchBpmnInstancesResult } from '../models/search-bpmn-instances-result.model';

export enum ActionTypes {
    CREATE_BPMNINSTANCE = "[BpmnInstances] CREATE_BPMNINSTANCE",
    COMPLETE_CREATE_BPMN_INSTANCE = "[BpmnInstances] COMPLETE_CREATE_BPMN_INSTANCE",
    ERROR_CREATE_BPMNINSTANCE = "[BpmnInstances] ERROR_CREATE_BPMNINSTANCE",
    START_BPMNINSTANCE = "[BpmnInstances] START_BPMNINSTANCE",
    COMPLETE_START_BPMNINSTANCE = "[BpmnInstances] COMPLETE_START_BPMNINSTANCE",
    ERROR_START_BPMNINSTANCE = "[BpmnInstances] ERROR_START_BPMNINSTANCE",
    SEARCH_BPMNINSTANCES = "[BpmnInstances] SEARCH_BPMNINSTANCES",
    COMPLETE_SEARCH_BPMNINSTANCES = "[BpmnInstances] COMPLETE_SEARCH_BPMNINSTANCES",
    ERROR_SEARCH_BPMNINSTANCES = "[BpmnInstances] ERROR_SEARCH_BPMNINSTANCES",
    GET_BPMNINSTANCE = "[BpmnInstances] GET_BPMNINSTANCE",
    COMPLETE_GET_BPMNINSTANCE = "[BpmnInstances] COMPLETE_GET_BPMNINSTANCE",
    ERROR_GET_BPMNINSTANCE = "[BpmnInstances] ERROR_GET_BPMNINSTANCE"
}

export class GetBpmnInstance implements Action {
    readonly type = ActionTypes.GET_BPMNINSTANCE
    constructor(public id: string) { }
}

export class CompleteGetBpmnInstance implements Action {
    readonly type = ActionTypes.COMPLETE_GET_BPMNINSTANCE
    constructor(public content: BpmnInstance) { }
}

export class CreateBpmnInstance implements Action {
    readonly type = ActionTypes.CREATE_BPMNINSTANCE
    constructor(public processFileId: string) { }
}

export class CompleteCreateBpmnInstance implements Action {
    readonly type = ActionTypes.COMPLETE_CREATE_BPMN_INSTANCE;
    constructor(public content: BpmnInstance) { }
}

export class StartBpmnInstance implements Action {
    readonly type = ActionTypes.START_BPMNINSTANCE
    constructor(public id: string) { }
}

export class CompleteStartBpmnInstance implements Action {
    readonly type = ActionTypes.COMPLETE_START_BPMNINSTANCE
    constructor() { }
}

export class SearchBpmnInstances implements Action {
    readonly type = ActionTypes.SEARCH_BPMNINSTANCES
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public processFileId: string) { }
}

export class CompleteSearchBpmnInstances implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_BPMNINSTANCES
    constructor(public content: SearchBpmnInstancesResult) { }
}

export type ActionsUnion = GetBpmnInstance |
    CompleteGetBpmnInstance |
    CreateBpmnInstance |
    CompleteCreateBpmnInstance |
    StartBpmnInstance |
    CompleteStartBpmnInstance |
    SearchBpmnInstances |
    CompleteSearchBpmnInstances;