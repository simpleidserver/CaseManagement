import { BpmnInstance } from './bpmn-instance.model';

export class SearchBpmnInstancesResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: BpmnInstance[];
}