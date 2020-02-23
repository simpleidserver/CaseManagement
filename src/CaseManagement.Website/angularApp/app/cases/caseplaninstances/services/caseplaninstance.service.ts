import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import { CasePlanInstance } from '../models/caseplaninstance.model';

@Injectable()
export class CasePlanInstanceService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchCasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCasePlanInstanceResult.fromJson(res);
        }));
    }

    searchMe(startIndex: number, count: number, order: string, direction: string): Observable<SearchCasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/me/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCasePlanInstanceResult.fromJson(res);
        }));
    }

    get(casePlanInstanceId: string): Observable<CasePlanInstance> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId;
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CasePlanInstance.fromJson(res);
        }));
    }
}