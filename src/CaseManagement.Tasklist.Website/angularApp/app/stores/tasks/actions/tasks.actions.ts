import { Action } from '@ngrx/store';
import { SearchTasksResult } from '../models/search-tasks-result.model';

export enum ActionTypes {
    SEARCH_TASKS = "[Tasks] SEARCH_TASKS",
    ERROR_SEARCH_TASKS = "[Tasks] ERROR_SEARCH_TASKS",
    COMPLETE_SEARCH_TASKS = "[Tasks] COMPLETE_SEARCH_TASKS"
}

export class SearchTasks implements Action {
    readonly type = ActionTypes.SEARCH_TASKS
    constructor(public order: string, public direction: string, public count: number, public startIndex: number, public owner: string, public status: string[]) { }
}

export class CompleteSearchTasks implements Action {
    readonly type = ActionTypes.COMPLETE_SEARCH_TASKS;
    constructor(public content: SearchTasksResult) { }
}

export type ActionsUnion = SearchTasks |
    CompleteSearchTasks;