import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CaseDefinition } from '../models/case-definition.model';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

const url = "http://localhost:54942";

@Injectable()
export class CaseDefinitionsService {
    constructor(private http: HttpClient) { }

    search(caseFileId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchCaseDefinitionsResult>{
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = url + "/case-definitions/.search?start_index=" + startIndex + "&count=" + count +" &case_file=" + caseFileId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCaseDefinitionsResult.fromJson(res);
        }));
    }

    get(id: string): Observable<CaseDefinition> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = url + "/case-definitions/" + id;
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CaseDefinition.fromJson(res);
        }));
    }
}