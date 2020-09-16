import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { CasePlanInstanceResult } from '../models/caseplaninstance.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';

@Injectable()
export class CasePlanInstanceService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, casePlanId: string): Observable<SearchCasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plan-instances/search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        if (casePlanId) {
            request["casePlanId"] = casePlanId;
        }

        return this.http.post<SearchCasePlanInstanceResult>(targetUrl, request, { headers: headers });
    }

    get(casePlanInstanceId: string): Observable<CasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId;
        return this.http.get<CasePlanInstanceResult>(targetUrl, { headers: headers });
    }

    createCasePlanInstance(casePlanId: string) : Observable<CasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances";
        const request: any = { casePlanId: casePlanId };
        return this.http.post<CasePlanInstanceResult>(targetUrl, request, { headers: headers });
    }

    launchCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/launch";
        return this.http.get(targetUrl, { headers: headers });
    }

    enable(casePlanInstanceId: string, casePlanElementInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/activate/" + casePlanElementInstanceId;
        return this.http.get(targetUrl, { headers: headers });
    }

    closeCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/close";
        return this.http.get(targetUrl, { headers: headers });
    }

    reactivateCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/reactivate";
        return this.http.get(targetUrl, { headers: headers });
    }

    resumeCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/resume";
        return this.http.get(targetUrl, { headers: headers });
    }

    suspendCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/suspend";
        return this.http.get(targetUrl, { headers: headers });
    }

    terminateCasePlanInstance(casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-plan-instances/" + casePlanInstanceId + "/terminate";
        return this.http.get(targetUrl, { headers: headers });
    }
}