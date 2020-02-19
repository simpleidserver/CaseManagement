import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CasePlan } from '../models/caseplan.model';
import { SearchCasePlanInstanceResult } from '../models/searchcaseplaninstanceresult.model';
import { SearchCasePlanResult } from '../models/searchcaseplanresult.model';
import { SearchFormInstanceResult } from '../models/searchforminstanceresult.model';
import { SearchWorkerTaskResult } from '../models/searchworkertaskresult.model';

@Injectable()
export class CasePlanService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, text: string): Observable<SearchCasePlanResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/me/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        if (text) {
            targetUrl = targetUrl + "&text=" + text;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return SearchCasePlanResult.fromJson(res);
        }));
    }

    get(id: string): Observable<CasePlan> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + id;
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CasePlan.fromJson(res);
        }));
    }

    searchHistory(casePlanId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchCasePlanResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/history/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            var result = SearchCasePlanResult.fromJson(res);
            return result;
        }));
    }

    searchCasePlanInstance(casePlanId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchCasePlanInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            var result = SearchCasePlanInstanceResult.fromJson(res);
            return result;
        }));
    }

    searchFormInstance(casePlanId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchFormInstanceResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/forminstances/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            var result = SearchFormInstanceResult.fromJson(res);
            return result;
        }));
    }

    searchWorkerTask(casePlanId: string, startIndex: number, count: number, order: string, direction: string): Observable<SearchWorkerTaskResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseworkertasks/search?start_index=" + startIndex + "&count=" + count;
        if (order) {
            targetUrl = targetUrl + "&order_by=" + order;
        }

        if (direction) {
            targetUrl = targetUrl + "&order=" + direction;
        }

        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            var result = SearchWorkerTaskResult.fromJson(res);
            return result;
        }));
    }

    launchCasePlanInstance(casePlanId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/launch";
        return this.http.get(targetUrl, { headers: headers });
    }

    closeCasePlanInstance(casePlanId: string, casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId +"/close";
        return this.http.get(targetUrl, { headers: headers });
    }

    reactivateCasePlanInstance(casePlanId: string, casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/reactivate";
        return this.http.get(targetUrl, { headers: headers });
    }

    resumeCasePlanInstance(casePlanId: string, casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/resume";
        return this.http.get(targetUrl, { headers: headers });
    }

    suspendCasePlanInstance(casePlanId: string, casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/suspend";
        return this.http.get(targetUrl, { headers: headers });
    }

    terminateCasePlanInstance(casePlanId: string, casePlanInstanceId: string) {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-plans/" + casePlanId + "/caseplaninstances/" + casePlanInstanceId + "/terminate";
        return this.http.get(targetUrl, { headers: headers });
    }
}