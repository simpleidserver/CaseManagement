export class Performance {
    CaptureDateTime: string;
    MachineName: string;
    NbWorkingThreads: number;
    MemoryConsumedMB: number;

    public static fromJson(json: any): Performance {
        var result = new Performance();
        result.CaptureDateTime = json["datetime"];
        result.MachineName = json["machine_name"];
        result.NbWorkingThreads = json["nb_working_threads"];
        result.MemoryConsumedMB = json["memory_consumed_mb"];
        return result;
    }
}