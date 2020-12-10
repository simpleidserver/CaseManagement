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
import * as fromActions from '../actions/bpmn-instances.actions';
export var initialBpmnInstancesLstState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export var initialBpmnInstanceState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function bpmnInstanceLstReducer(state, action) {
    if (state === void 0) { state = initialBpmnInstancesLstState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_SEARCH_BPMNINSTANCES:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
export function bpmnInstanceReducer(state, action) {
    if (state === void 0) { state = initialBpmnInstanceState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_BPMNINSTANCE:
            state.content = action.content;
            return __assign({}, state);
        default:
            return state;
    }
}
//# sourceMappingURL=bpmninstance.reducers.js.map