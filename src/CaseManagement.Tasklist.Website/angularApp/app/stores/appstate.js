import { createSelector } from '@ngrx/store';
import * as fromTask from './tasks/reducers/tasks.reducers';
export var selectTaskLst = function (state) { return state.taskLst; };
export var selectTaskLstResult = createSelector(selectTaskLst, function (state) {
    if (!state || state.content == null) {
        return null;
    }
    return state.content;
});
export var appReducer = {
    taskLst: fromTask.taskLstReducer
};
//# sourceMappingURL=appstate.js.map