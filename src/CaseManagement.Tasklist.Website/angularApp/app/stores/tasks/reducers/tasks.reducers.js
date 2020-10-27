var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
import * as fromActions from '../actions/tasks.actions';
export var initiaTaskLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function taskLstReducer(state, action) {
    if (state === void 0) { state = initiaTaskLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_TASKS:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=tasks.reducers.js.map