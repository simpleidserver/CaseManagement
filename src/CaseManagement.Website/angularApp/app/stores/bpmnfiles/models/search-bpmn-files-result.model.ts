import { BpmnFile } from './bpmn-file.model';

export class SearchBpmnFilesResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: BpmnFile[];
}