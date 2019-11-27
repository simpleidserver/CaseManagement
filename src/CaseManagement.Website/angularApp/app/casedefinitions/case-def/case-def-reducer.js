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
import { ActionTypes } from './case-def-actions';
var initialCaseDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function reducer(state, action) {
    if (state === void 0) { state = initialCaseDefState; }
    switch (action.type) {
        case ActionTypes.CASEDEFLOADED:
            var caseDefLoadedAction = action;
            state.content = caseDefLoadedAction.result;
            state.isLoading = false;
            state.isErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEDEF:
            state.isErrorLoadOccured = true;
            state.isLoading = false;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=case-def-reducer.js.map