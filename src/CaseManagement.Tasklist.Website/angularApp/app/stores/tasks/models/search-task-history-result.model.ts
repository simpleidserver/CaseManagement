import { TaskHistory } from './task-history.model';

export class SearchTaskHistoryResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: TaskHistory[];
}