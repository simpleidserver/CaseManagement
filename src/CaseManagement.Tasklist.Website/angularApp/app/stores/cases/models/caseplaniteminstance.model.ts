import { TransitionHistory } from "./transitionhistory.model";

export class CasePlanItemInstance {
    constructor() {
        this.transitionHistories = [];    
    }

    id: string;
    eltId: string;
    nbOccurrence: number;
    name: string;
    type: string;
    parentEltId: string;
    state: string;
    transitionHistories: TransitionHistory[];
}