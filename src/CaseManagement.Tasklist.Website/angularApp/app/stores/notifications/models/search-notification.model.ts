import { Notification } from './notification.model';

export class SearchNotificationResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: Notification[];
}