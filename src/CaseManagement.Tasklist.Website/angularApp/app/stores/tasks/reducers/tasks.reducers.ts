import * as fromActions from '../actions/tasks.actions';
import { RenderingElement } from '../models/rendering';
import { SearchTaskHistoryResult } from '../models/search-task-history-result.model';
import { SearchTasksResult } from "../models/search-tasks-result.model";
import { Task } from '../models/task.model';

export interface TaskLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchTasksResult;
}

export interface TaskState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    renderingElts: RenderingElement[];
    description: string;
    searchTaskHistory: SearchTaskHistoryResult;
    task: Task;
}

export const initialTaskLstState: TaskLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export const initialTaskState: TaskState = {
    renderingElts: null,
    description: null,
    task: null,
    searchTaskHistory: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function taskLstReducer(state = initialTaskLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_TASKS:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}

export function taskReducer(state = initialTaskState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_TASK:
            state.renderingElts = action.renderingElts;
            state.task = action.task;
            state.description = action.description;
            state.searchTaskHistory = action.searchTaskHistory;
            return { ...state };
        default:
            return state;
    }
}