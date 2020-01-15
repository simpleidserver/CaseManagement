import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SearchPerformancesResult } from '../models/search-performances-result.model';

@Injectable()
export class StatisticService {
    constructor(private http: HttpClient) { }

    getPerformances(): Observable<string[]> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/statistics/performances";
        return this.http.get<string[]>(targetUrl, { headers: headers });
    }

    searchPerformances(startIndex: number, count: number, order: string, direction: string, startDateTime: string): Observable<SearchPerformancesResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/statistics/performances/.search?start_index=" + startIndex + "&count=" + count + "&group_by=machine_name";
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        if (startDateTime) {
            targetUrl = targetUrl + "&start_datetime=" + startDateTime;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchPerformancesResult.fromJson(res);
        }));
    }
}