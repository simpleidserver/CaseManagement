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
import { ActionTypes } from "./case-instance-actions";
var initialCaseInstanceState = {
    caseDefinition: null,
    caseInstance: null,
    executionStepsResult: null,
    isCaseInstanceLoading: true,
    isCaseInstanceErrorLoadOccured: false,
    isCaseExecutionStepsLoading: true,
    isCaseExecutionStepsErrorLoadOccured: false
};
export function reducer(state, action) {
    if (state === void 0) { state = initialCaseInstanceState; }
    switch (action.type) {
        case ActionTypes.CASEINSTANCELOADED:
            var caseInstanceLoadedAction = action;
            state.caseDefinition = caseInstanceLoadedAction.caseDefinition;
            state.caseInstance = caseInstanceLoadedAction.caseInstance;
            state.isCaseInstanceLoading = false;
            state.isCaseInstanceErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEINSTANCE:
            state.isCaseInstanceLoading = false;
            state.isCaseInstanceErrorLoadOccured = true;
            return __assign({}, state);
        case ActionTypes.CASEEXECUTIONSTEPSLOADED:
            var caseExecutionStepsLoadedAction = action;
            state.executionStepsResult = caseExecutionStepsLoadedAction.result;
            state.isCaseExecutionStepsLoading = false;
            state.isCaseExecutionStepsErrorLoadOccured = false;
            return __assign({}, state);
        case ActionTypes.ERRORLOADCASEEXECUTIONSTEPS:
            state.isCaseExecutionStepsLoading = false;
            state.isCaseExecutionStepsErrorLoadOccured = false;
            return __assign({}, state);
    }
}
//# sourceMappingURL=case-instance-reducer.js.map