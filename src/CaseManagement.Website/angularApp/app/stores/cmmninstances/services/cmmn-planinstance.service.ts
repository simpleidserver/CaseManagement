import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { CmmnPlanInstanceResult } from '../models/cmmn-planinstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcmmnplaninstanceresult.model';

@Injectable()
export class CmmnPlanInstanceService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, casePlanId: string, caseFileId: string): Observable<SearchCasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/search";
        const request: any = { startIndex: startIndex, count: count, casePlanId: casePlanId, caseFileId: caseFileId };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchCasePlanInstanceResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    createCasePlanInstance(casePlanId: string): Observable<CmmnPlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances";
        const request: any = { casePlanId: casePlanId };
        return this.http.post<CmmnPlanInstanceResult>(targetUrl, request, { headers: headers });
    }

    launchCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/launch";
        return this.http.get(targetUrl, { headers: headers });
    }

    get(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId;
        return this.http.get(targetUrl, { headers: headers });
    }
}