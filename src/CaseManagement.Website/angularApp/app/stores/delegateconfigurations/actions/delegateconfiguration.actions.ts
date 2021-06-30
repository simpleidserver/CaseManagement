import { Action } from '@ngrx/store';
import { DelegateConfiguration } from '../models/delegateconfiguration.model';
import { SearchDelegateConfigurationResult } from '../models/searchdelegateconfiguration.model';

export enum ActionTypes {
    SEARCH_DELEGATE_CONFIGURATION = "[DelegateConfigurations] SEARCH_DELEGATE_CONFIGURATION",
    ERROR_SEARCH_DELEGATE_CONFIGURATION = "[DelegateConfigurations] ERROR_SEARCH_DELEGATE_CONFIGURATION",
    COMPLETE_SEARCH_DELEGATE_CONFIGURATION = "[DelegateConfigurations] COMPLETE_SEARCH_CMMN_PLANINSTANCE",
    GET_DELEGATE_CONFIGURATION = "[DelegateConfiguration] GET_DELEGATE_CONFIGURATION",
    ERROR_GET_DELEGATE_CONFIGURATION = "[DelegateConfiguration] ERROR_GET_DELEGATE_CONFIGURATION",
    COMPLETE_GET_DELEGATE_CONFIGURATION = "[DelegateConfiguration] COMPLETE_GET_DELEGATE_CONFIGURATION",
    START_UPDATE_DELEGATE_CONFIGURATION = "[DelegateConfiguration] START_UPDATE_DELEGATE_CONFIGURATION",
    ERROR_UPDATE_DELEGATE_CONFIGURATION = "[DelegateConfiguration] ERROR_UPDATE_DELEGATE_CONFIGURATION",
    COMPLETE_UPDATE_DELEGATE_CONFIGURATION = "[DelegateConfiguration] COMPLETE_UPDATE_DELEGATE_CONFIGURATION",
    START_GET_ALL_DELEGATE_CONFIGURATION = "[DelegateConfiguration] START_GET_ALL_DELEGATE_CONFIGURATION",
    ERROR_GET_ALL_DELEGATE_CONFIGURATION = "[DelegateConfiguration] ERROR_GET_ALL_DELEGATE_CONFIGURATION",
    COMPLETE_GET_ALL_DELEGATE_CONFIGURATION = "[DelegateConfiguration] COMPLETE_GET_ALL_DELEGATE_CONFIGURATION"
}

export class SearchDelegateConfiguration implements Action {
    readonly type = ActionTypes.SEARCH_DELEGATE_CONFIGURATION;
    constructor(public order: string, public direction: string, public count: number, public startIndex: number) { }
}

export class CompleteSearchDelegateConfiguration implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_DELEGATE_CONFIGURATION;
    constructor(public content: SearchDelegateConfigurationResult) { }
}

export class GetDelegateConfiguration implements Action {
    readonly type = ActionTypes.GET_DELEGATE_CONFIGURATION;
    constructor(public id: string) { }
}

export class CompleteGetDelegateConfiguration implements Action {
    readonly type = ActionTypes.COMPLETE_GET_DELEGATE_CONFIGURATION;
    constructor(public content: DelegateConfiguration) { }
}

export class UpdateDelegateConfiguration implements Action {
    readonly type = ActionTypes.START_UPDATE_DELEGATE_CONFIGURATION;
    constructor(public id: string, public records: any) { }
}

export class GetAllDelegateConfiguration implements Action {
    readonly type = ActionTypes.START_GET_ALL_DELEGATE_CONFIGURATION;
    constructor() { }
}

export class CompleteGetAllDelegateConfiguration implements Action {
    readonly type = ActionTypes.COMPLETE_GET_ALL_DELEGATE_CONFIGURATION;
    constructor(public content: string[]) { }
}

export type ActionsUnion = SearchDelegateConfiguration |
    CompleteSearchDelegateConfiguration |
    GetDelegateConfiguration |
    CompleteGetDelegateConfiguration |
    UpdateDelegateConfiguration |
    GetAllDelegateConfiguration |
    CompleteGetAllDelegateConfiguration;