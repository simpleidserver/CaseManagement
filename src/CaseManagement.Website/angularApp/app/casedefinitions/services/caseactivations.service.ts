import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SearchCaseActivationsResult } from '../models/search-case-activations-result.model';

@Injectable()
export class CaseActivationsService {
    constructor(private http: HttpClient) { }

    search(caseDefinitionId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchCaseActivationsResult>{
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-activations/.search?start_index=" + startIndex + "&count=" + count + "&case_definition_id=" + caseDefinitionId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCaseActivationsResult.fromJson(res);
        }));
    }
}