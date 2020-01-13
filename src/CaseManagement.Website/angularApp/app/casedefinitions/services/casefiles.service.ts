import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

@Injectable()
export class CaseFilesService {
    constructor(private http: HttpClient) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchCaseFilesResult>{
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-files/.search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCaseFilesResult.fromJson(res);
        }));
    }

    get(id: string): Observable<CaseFile> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-files/" + id;
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CaseFile.fromJson(res);
        }));
    }
}