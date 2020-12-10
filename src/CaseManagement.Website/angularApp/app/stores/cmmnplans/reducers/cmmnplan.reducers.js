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
import * as fromActions from '../actions/cmmn-plans.actions';
export var initialCmmnPlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialSearchCmmnPlanState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function cmmnPlanReducer(state, action) {
    if (state === void 0) { state = initialCmmnPlanState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_CMMN_PLAN:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function cmmnPlanLstReducer(state, action) {
    if (state === void 0) { state = initialSearchCmmnPlanState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_CMMN_PLANS:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=cmmnplan.reducers.js.map