import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SearchCaseInstancesResult } from '../models/search-case-instances-result.model';

const url = "http://localhost:54942";

@Injectable()
export class CaseInstancesService {
    constructor(private http: HttpClient) { }

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