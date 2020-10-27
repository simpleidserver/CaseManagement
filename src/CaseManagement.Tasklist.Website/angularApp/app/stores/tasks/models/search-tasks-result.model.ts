import { Task } from './task.model';

export class SearchTasksResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: Task[];
}