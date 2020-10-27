import * as fromActions from '../actions/tasks.actions';
import { SearchTasksResult } from "../models/search-tasks-result.model";

export interface TaskLstState {
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: SearchTasksResult;
}

export const initiaTaskLstState: TaskLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function taskLstReducer(state = initiaTaskLstState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_TASKS:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
}