import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { CmmnPlan } from '../models/cmmn-plan.model';
import { SearchCmmnPlanResult } from '../models/searchcmmnplanresult.model';

@Injectable()
export class CmmnPlanService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, caseFileId: string, takeLatest: boolean): Observable<SearchCmmnPlanResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        let targetUrl = process.env.API_URL + "/case-plans/search";
        const request: any = { startIndex: startIndex, count: count, takeLatest: takeLatest };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        if (caseFileId) {
            request["caseFileId"] = caseFileId;
        }

        return this.http.post<SearchCmmnPlanResult>(targetUrl, request, { headers: headers });
    }

    get(id: string): Observable<CmmnPlan> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + id;
        return this.http.get<CmmnPlan>(targetUrl, { headers: headers });
    }
}