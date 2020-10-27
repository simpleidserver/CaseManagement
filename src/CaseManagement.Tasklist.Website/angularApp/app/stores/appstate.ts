import { createSelector } from '@ngrx/store';
import * as fromTask from './tasks/reducers/tasks.reducers';

export interface AppState {
    taskLst: fromTask.TaskLstState;
}

export const selectTaskLst = (state: AppState) => state.taskLst;

export const selectTaskLstResult = createSelector(
    selectTaskLst,
    (state: fromTask.TaskLstState) => {
        if (!state || state.content == null) {
            return null;
        }

        return state.content;
    }
);

export const appReducer = {
    taskLst: fromTask.taskLstReducer
};