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
import { CaseInstanceState } from "./case-instance-states";
import { ActionTypes } from "./case-instance-actions";
var initialCaseInstanceState = new CaseInstanceState();
export function reducer(state, action) {
    if (state === void 0) { state = initialCaseInstanceState; }
    switch (action.type) {
        case ActionTypes.CASEINSTANCELOADED:
            var caseInstanceLoadedAction = action;
            initialCaseInstanceState.caseDefinition = caseInstanceLoadedAction.caseDefinition;
            initialCaseInstanceState.caseInstance = caseInstanceLoadedAction.caseInstance;
            initialCaseInstanceState.isCaseInstanceLoading = false;
            initialCaseInstanceState.isCaseInstanceErrorLoadOccured = false;
            return __assign({}, initialCaseInstanceState);
        case ActionTypes.ERRORLOADCASEINSTANCE:
            initialCaseInstanceState.isCaseInstanceLoading = false;
            initialCaseInstanceState.isCaseInstanceErrorLoadOccured = true;
            return __assign({}, initialCaseInstanceState);
        case ActionTypes.CASEEXECUTIONSTEPSLOADED:
            console.log(state);
            var caseExecutionStepsLoadedAction = action;
            initialCaseInstanceState.executionStepsResult = caseExecutionStepsLoadedAction.result;
            initialCaseInstanceState.isCaseExecutionStepsLoading = false;
            initialCaseInstanceState.isCaseExecutionStepsErrorLoadOccured = false;
            return __assign({}, initialCaseInstanceState);
        case ActionTypes.ERRORLOADCASEEXECUTIONSTEPS:
            initialCaseInstanceState.isCaseExecutionStepsLoading = false;
            initialCaseInstanceState.isCaseExecutionStepsErrorLoadOccured = false;
            return __assign({}, initialCaseInstanceState);
    }
}
//# sourceMappingURL=case-instance-reducer.js.map