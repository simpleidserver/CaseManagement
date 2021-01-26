import { NotificationDefinition } from "./notificationdef.model";

export class SearchNotificationDefsResult {
    constructor() {
        this.content = [];
    }

    startIndex: number;
    totalLength: number;
    count: string;
    content: NotificationDefinition[];
}