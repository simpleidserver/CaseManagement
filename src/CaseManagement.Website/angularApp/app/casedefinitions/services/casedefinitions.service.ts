import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CaseDefinitionHistory } from '../models/case-definition-history.model';
import { CaseDefinition } from '../models/case-definition.model';
import { CountResult } from '../models/count-result.model';
import { SearchCaseDefinitionsResult } from '../models/search-case-definitions-result.model';

@Injectable()
export class CaseDefinitionsService {
    constructor(private http: HttpClient) { }

    search(startIndex: number, count: number, order: string, direction: string, text: string): Observable<SearchCaseDefinitionsResult>{
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-definitions/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        if (text) {
            targetUrl = targetUrl + "&text=" + text;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCaseDefinitionsResult.fromJson(res);
        }));
    }

    get(id: string): Observable<CaseDefinition> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-definitions/" + id;
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CaseDefinition.fromJson(res);
        }));
    }

    getHistory(id: string): Observable<CaseDefinitionHistory> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-definitions/" + id + "/history";
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CaseDefinitionHistory.fromJson(res);
        }));
    }

    count(): Observable<CountResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-definitions/count";
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CountResult.fromJson(res);
        }));        
    }
}