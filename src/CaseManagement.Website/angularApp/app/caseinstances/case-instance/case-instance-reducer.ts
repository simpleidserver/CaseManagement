import { CaseInstanceState } from "./case-instance-states";
import { ActionsUnion, ActionTypes, CaseInstanceLoadedAction, CaseExecutionStepsLoadedAction } from "./case-instance-actions";

const initialCaseInstanceState: CaseInstanceState = {
    caseDefinition: null,
    caseInstance: null,
    executionStepsResult: null,
    isCaseInstanceLoading: true,
    isCaseInstanceErrorLoadOccured: false,
    isCaseExecutionStepsLoading: true,
    isCaseExecutionStepsErrorLoadOccured: false
};

export function reducer(state = initialCaseInstanceState, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEINSTANCELOADED:
            let caseInstanceLoadedAction = <CaseInstanceLoadedAction>action;
            state.caseDefinition = caseInstanceLoadedAction.caseDefinition;
            state.caseInstance = caseInstanceLoadedAction.caseInstance;
            state.isCaseInstanceLoading = false;
            state.isCaseInstanceErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEINSTANCE:
            state.isCaseInstanceLoading = false;
            state.isCaseInstanceErrorLoadOccured = true;
            return { ...state };
        case ActionTypes.CASEEXECUTIONSTEPSLOADED:
            let caseExecutionStepsLoadedAction = <CaseExecutionStepsLoadedAction>action;
            state.executionStepsResult = caseExecutionStepsLoadedAction.result;
            state.isCaseExecutionStepsLoading = false;
            state.isCaseExecutionStepsErrorLoadOccured = false;
            return { ...state };
        case ActionTypes.ERRORLOADCASEEXECUTIONSTEPS:
            state.isCaseExecutionStepsLoading = false;
            state.isCaseExecutionStepsErrorLoadOccured = false;
            return { ...state };
    }
}