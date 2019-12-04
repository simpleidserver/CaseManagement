import { CaseInstanceState } from "./case-instance-states";
import { ActionsUnion, ActionTypes, CaseInstanceLoadedAction, CaseExecutionStepsLoadedAction } from "./case-instance-actions";


const initialCaseInstanceState = new CaseInstanceState();

export function reducer(state = initialCaseInstanceState, action: ActionsUnion) {
    switch (action.type) {
        case ActionTypes.CASEINSTANCELOADED:
            let caseInstanceLoadedAction = <CaseInstanceLoadedAction>action;
            initialCaseInstanceState.caseDefinition = caseInstanceLoadedAction.caseDefinition;
            initialCaseInstanceState.caseInstance = caseInstanceLoadedAction.caseInstance;
            initialCaseInstanceState.isCaseInstanceLoading = false;
            initialCaseInstanceState.isCaseInstanceErrorLoadOccured = false;
            return { ...initialCaseInstanceState };
        case ActionTypes.ERRORLOADCASEINSTANCE:
            initialCaseInstanceState.isCaseInstanceLoading = false;
            initialCaseInstanceState.isCaseInstanceErrorLoadOccured = true;
            return { ...initialCaseInstanceState };
        case ActionTypes.CASEEXECUTIONSTEPSLOADED:
            console.log(state);
            let caseExecutionStepsLoadedAction = <CaseExecutionStepsLoadedAction>action;
            initialCaseInstanceState.executionStepsResult = caseExecutionStepsLoadedAction.result;
            initialCaseInstanceState.isCaseExecutionStepsLoading = false;
            initialCaseInstanceState.isCaseExecutionStepsErrorLoadOccured = false;
            return { ...initialCaseInstanceState };
        case ActionTypes.ERRORLOADCASEEXECUTIONSTEPS:
            initialCaseInstanceState.isCaseExecutionStepsLoading = false;
            initialCaseInstanceState.isCaseExecutionStepsErrorLoadOccured = false;
            return { ...initialCaseInstanceState };
    }
}