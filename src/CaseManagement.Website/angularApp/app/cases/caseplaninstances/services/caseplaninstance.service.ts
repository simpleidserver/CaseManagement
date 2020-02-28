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

    search(startIndex: number, count: number, order: string, direction: string, casePlanId: string): Observable<SearchCasePlanInstanceResult> {
        var claims: any = this.oauthService.getIdentityClaims();
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/search?start_index=" + startIndex + "&count=" + count;
        if (claims.role === 'caseworker') {
            targetUrl = process.env.API_URL + "/case-plan-instances/search/me?start_index=" + startIndex + "&count=" + count;
        }

        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        if (casePlanId) {
            targetUrl = targetUrl + "&case_plan_id=" + casePlanId;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCasePlanInstanceResult.fromJson(res);
        }));
    }

    get(casePlanInstanceId: string): Observable<CasePlanInstance> {
        var claims: any = this.oauthService.getIdentityClaims();
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId;
        if (claims.role === 'caseworker') {
            targetUrl = process.env.API_URL + "/case-plan-instances/me/" + casePlanInstanceId;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CasePlanInstance.fromJson(res);
        }));
    }

    enable(casePlanInstanceId: string, casePlanElementInstanceId: string) {
        var claims: any = this.oauthService.getIdentityClaims();
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/enable/" + casePlanElementInstanceId;
        if (claims.role === 'caseworker') {
            targetUrl = process.env.API_URL + "/case-plan-instances/me/" + casePlanInstanceId + "/enable/" + casePlanElementInstanceId;
        }

        return this.http.get(targetUrl, { headers: headers });
    }

    confirmForm(casePlanInstanceId: string, casePlanElementInstanceId: string, request: any) {
        var claims: any = this.oauthService.getIdentityClaims();
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/confirm/" + casePlanElementInstanceId;
        if (claims.role === 'caseworker') {
            targetUrl = process.env.API_URL + "/case-plan-instances/me/" + casePlanInstanceId + "/confirm/" + casePlanElementInstanceId;
        }

        return this.http.post(targetUrl, JSON.stringify(request), { headers: headers });
    }
}