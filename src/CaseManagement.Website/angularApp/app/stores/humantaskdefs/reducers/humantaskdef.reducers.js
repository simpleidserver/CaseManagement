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
var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
import * as fromActions from '../actions/humantaskdef.actions';
export var initialHumanTaskDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};
export function humanTaskDefReducer(state, action) {
    if (state === void 0) { state = initialHumanTaskDefState; }
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_HUMANTASKDEF:
            state.content = action.content;
            return __assign({}, state);
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF:
            state.content = action.content;
            return __assign({}, state);
        case fromActions.ActionTypes.COMPLETE_ADD_START_DEADLINE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { startDeadLines: __spreadArrays(state.content.deadLines.startDeadLines, [
                            action.content
                        ]) }) }) });
        case fromActions.ActionTypes.COMPLETE_ADD_COMPLETION_DEADLINE:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { deadLines: __assign(__assign({}, state.content.deadLines), { completionDeadLines: __spreadArrays(state.content.deadLines.completionDeadLines, [
                            action.content
                        ]) }) }) });
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANTASK_INFO:
            return __assign(__assign({}, state), { content: __assign(__assign({}, state.content), { name: action.name, priority: action.priority }) });
        default:
            return state;
    }
}
;
//# sourceMappingURL=humantaskdef.reducers.js.map