import { CaseDefinition } from "../models/case-definition.model";

export interface CaseDefinitionState {
	isLoading: boolean;
	isErrorLoadOccured: boolean;
    caseDefinition: CaseDefinition;
}