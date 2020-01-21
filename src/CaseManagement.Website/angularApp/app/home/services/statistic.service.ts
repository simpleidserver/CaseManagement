import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { DailyStatistic } from '../models/dailystatistic.model';
import { SearchDailyStatisticsResult } from '../models/search-dailystatistics-result.model';

@Injectable()
export class StatisticService {
    constructor(private http: HttpClient) { }

    get(): Observable<DailyStatistic>{
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/statistics";
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return DailyStatistic.fromJson(res);
        }));
    }

    search(startIndex: number, count: number, order: string, direction: string, startDate: string, endDate: string): Observable<SearchDailyStatisticsResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/statistics/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        if (startDate) {
            targetUrl = targetUrl + "&start_datetime=" + startDate;
        }

        if (endDate) {
            targetUrl = targetUrl + "&end_datetime=" + endDate;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchDailyStatisticsResult.fromJson(res);
        }));
    }
}