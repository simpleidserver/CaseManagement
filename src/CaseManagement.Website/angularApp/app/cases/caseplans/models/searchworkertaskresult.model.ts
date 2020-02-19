import { WorkerTask } from './workertask.model';

export class SearchWorkerTaskResult {
    StartIndex: number;
    Count: number;
    TotalLength: number;
    Content: WorkerTask[];

    public static fromJson(json: any): SearchWorkerTaskResult
    {
        let result = new SearchWorkerTaskResult();
        result.StartIndex = json["start_index"];
        result.Count = json["count"];
        result.TotalLength = json["total_length"];
        let content: WorkerTask[] = [];
        if (json["content"]) {
            json["content"].forEach(function (c: any) {
                content.push(WorkerTask.fromJson(c));
            });
        }

        result.Content = content;
        return result;
    }
}