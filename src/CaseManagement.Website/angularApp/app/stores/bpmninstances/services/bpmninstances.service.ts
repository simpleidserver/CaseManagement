import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { BpmnInstance } from '../models/bpmn-instance.model';
import { SearchBpmnInstancesResult } from '../models/search-bpmn-instances-result.model';

@Injectable()
export class BpmnInstancesService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, processFileId: string): Observable<SearchBpmnInstancesResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.BPMN_API_URL + "/processinstances/search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        if (processFileId) {
            request["processFileId"] = processFileId;
        }

        return this.http.post<SearchBpmnInstancesResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    get(id: string): Observable<BpmnInstance> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.BPMN_API_URL + "/processinstances/" + id;
        return this.http.get<BpmnInstance>(targetUrl, { headers: headers });
    }

    create(processFileId: string): Observable<BpmnInstance> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.BPMN_API_URL + "/processinstances";
        const request: any = { processFileId: processFileId };
        return this.http.post<BpmnInstance>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    start(id: string): Observable<any> {
        let headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getAccessToken());
        const targetUrl = process.env.BPMN_API_URL + "/processinstances/" + id + "/start";
        return this.http.get<any>(targetUrl, { headers: headers });
    }
}