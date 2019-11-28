import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SearchCaseInstancesResult, CaseInstance } from '../models/search-case-instances-result.model';

const url = "http://localhost:54942";

@Injectable()
export class CaseInstancesService {
    constructor(private http: HttpClient) { }

    create(caseDefId: string, caseId: string) : Observable<CaseInstance> {
        const request = JSON.stringify({ case_definition_id: caseDefId, case_id: caseId });
        let targetUrl = url + "/case-instances";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        return this.http.post<Observable<CaseInstance>>(targetUrl, request, { headers: headers }).pipe(map((res: any) => {
            return CaseInstance.fromJson(res);
        }));
    }

    launch(caseInstanceId: string) {
        let targetUrl = url + "/case-instances/" + caseInstanceId + "/launch";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        return this.http.get(targetUrl, { headers: headers });
    }

    search(startIndex: number, count: number, templateId: string, order: string, direction: string): Observable<SearchCaseInstancesResult>{
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = url + "/case-instances/.search?start_index=" + startIndex + "&count=" + count + "&template_id=" + templateId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCaseInstancesResult.fromJson(res);
        }));
    }
}