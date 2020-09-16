import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { CasePlan } from '../models/caseplan.model';
import { SearchCasePlanResult } from '../models/searchcaseplanresult.model';

@Injectable()
export class CasePlanService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, text: string, caseFileId: string): Observable<SearchCasePlanResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/search";
        const request: any = { startIndex: startIndex, count: count, takeLatest: true };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        if (text) {
            request["text"] = text;
        }

        if (caseFileId) {
            request["caseFileId"] = caseFileId;
        }

        return this.http.post<SearchCasePlanResult>(targetUrl, request, { headers: headers });
    }

    searchHistory(casePlanId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchCasePlanResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/search";
        const request: any = { startIndex: startIndex, count: count, casePlanId: casePlanId, takeLatest: false };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchCasePlanResult>(targetUrl, request, { headers: headers });
    }

    get(id: string): Observable<CasePlan> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + id;
        return this.http.get<CasePlan>(targetUrl, { headers: headers });
    }
}