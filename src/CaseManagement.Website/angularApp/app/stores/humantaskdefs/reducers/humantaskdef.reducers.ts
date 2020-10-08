import * as fromActions from '../actions/humantaskdef.actions';
import { HumanTaskDef } from '../models/humantaskdef.model';

export interface HumanTaskDefState{
    isLoading: boolean;
    isErrorLoadOccured: boolean;
    content: HumanTaskDef;
}

export const initialHumanTaskDefState: HumanTaskDefState = {
    content: null,
    isLoading: true,
    isErrorLoadOccured: false
};

export function humanTaskDefReducer(state = initialHumanTaskDefState, action: fromActions.ActionsUnion) {
    switch (action.type) {
        case fromActions.ActionTypes.COMPLETE_GET_HUMANTASKDEF:
            state.content = action.content;
            return { ...state };
        case fromActions.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF:
            state.content = action.content;
            return { ...state };
        default:
            return state;
    }
};