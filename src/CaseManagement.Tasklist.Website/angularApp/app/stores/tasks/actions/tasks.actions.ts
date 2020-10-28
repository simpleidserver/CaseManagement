import { Action } from '@ngrx/store';
import { SearchTasksResult } from '../models/search-tasks-result.model';
import { NominateParameter } from '../parameters/nominate-parameter';

export enum ActionTypes {
    SEARCH_TASKS = "[Tasks] SEARCH_TASKS",
    ERROR_SEARCH_TASKS = "[Tasks] ERROR_SEARCH_TASKS",
    COMPLETE_SEARCH_TASKS = "[Tasks] COMPLETE_SEARCH_TASKS",
    START_TASK = "[Tasks] START_TASK",
    ERROR_START_TASK = "[Tasks] ERROR_START_TASK",
    COMPLETE_START_TASK = "[Tasks] COMPLETE_START_TASK",
    NOMINATE_TASK = "[Tasks] NOMINATE_TASK",
    ERROR_NOMINATE_TASK = "[Tasks] ERROR_NOMINATE_TASK",
    COMPLETE_NOMINATE_TASK = "[Tasks] COMPLETE_NOMINATE_TASK",
    CLAIM_TASK = "[Tasks] CLAIM_TASK",
    ERROR_CLAIM_TASK = "[Tasks] ERROR_CLAIM_TASK",
    COMPLETE_CLAIM_TASK = "[Tasks] COMPLETE_CLAIM_TASK"
}

export class SearchTasks implements Action {
    readonly type = ActionTypes.SEARCH_TASKS
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public owner: string, public status: string[]) { }
}

export class CompleteSearchTasks implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_TASKS;
    constructor(public content: SearchTasksResult) { }
}

export class StartTask implements Action {
    readonly type = ActionTypes.START_TASK
    constructor(public humanTaskInstanceId: string) { }
}

export class NominateTask implements Action {
    readonly type = ActionTypes.NOMINATE_TASK
    constructor(public humanTaskInstanceId: string, public parameter: NominateParameter) { }
}

export class ClaimTask implements Action {
    readonly type = ActionTypes.CLAIM_TASK
    constructor(public humanTaskInstanceId: string) { }
}

export type ActionsUnion = SearchTasks |
    CompleteSearchTasks |
    StartTask |
    NominateTask |
    ClaimTask;