import { CmmnFile } from './cmmn-file.model';

export class SearchCmmnFilesResult {
    startIndex: number;
    count: number;
    totalLength: number;
    content: CmmnFile[];
}