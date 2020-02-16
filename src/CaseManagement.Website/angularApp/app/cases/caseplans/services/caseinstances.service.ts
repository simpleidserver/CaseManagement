import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SearchCaseInstancesResult } from '../models/search-case-instances.model';
import { CaseInstance } from '../models/case-instance.model';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class CaseInstancesService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(caseDefinitionId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchCaseInstancesResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-instances/search?start_index=" + startIndex + "&count=" + count + "&case_definition_id=" + caseDefinitionId;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            var result = SearchCaseInstancesResult.fromJson(res);
            return result;
        }));
    }

    create(caseDefId: string): Observable<CaseInstance> {
        const request = JSON.stringify({ case_definition_id: caseDefId });
        let targetUrl = process.env.API_URL + "/case-instances";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.post<Observable<CaseInstance>>(targetUrl, request, { headers: headers }).pipe(map((res: any) => {
            return CaseInstance.fromJson(res);
        }));
    }

    launch(caseInstanceId: string) {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/launch";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    }

    reactivateCaseInstance(caseInstanceId: string) {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/reactivate";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    }

    suspendCaseInstance(caseInstanceId: string) {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/suspend";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        return this.http.get(targetUrl, { headers: headers });
    }

    resumeCaseInstance(caseInstanceId: string) {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/resume";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    }

    closeCaseInstance(caseInstanceId: string) {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/close";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    }

    getCaseFileItems(caseInstanceId: string) {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId + "/casefileitems";
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers });
    }

    get(caseInstanceId: string): Observable<CaseInstance> {
        let targetUrl = process.env.API_URL + "/case-instances/" + caseInstanceId;
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            var result = CaseInstance.fromJson(res);
            return result;
        }));
    }
}