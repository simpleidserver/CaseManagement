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
import * as fromActions from '../actions/caseplaninstance.actions';
export var initialCasePlanInstanceState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialCasePlanInstanceLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function casePlanInstanceReducer(state, action) {
    if (state === void 0) { state = initialCasePlanInstanceState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CASE_PLANINSTANCE:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
;
export function casePlanInstanceLstReducer(state, action) {
    if (state === void 0) { state = initialCasePlanInstanceLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CASE_PLANINSTANCES:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=caseplaninstance.reducers.js.map