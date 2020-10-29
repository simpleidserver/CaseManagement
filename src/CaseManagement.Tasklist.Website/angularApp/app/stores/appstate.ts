import { createSelector } from '@ngrx/store';
import * as fromTask from './tasks/reducers/tasks.reducers';

export interface AppState {
    taskLst: fromTask.TaskLstState;
    task: fromTask.TaskState;
}

export const selectTaskLst = (state: AppState) => state.taskLst;
export const selectTask = (state: AppState) => state.task;

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
    taskLst: fromTask.taskLstReducer,
    task: fromTask.taskReducer
};