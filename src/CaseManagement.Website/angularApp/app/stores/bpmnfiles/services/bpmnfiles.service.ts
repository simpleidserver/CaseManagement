import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { SearchBpmnFilesResult } from '../models/search-bpmn-files-result.model';
import { BpmnFile } from '../models/bpmn-file.model';

@Injectable()
export class BpmnFilesService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, takeLatest: boolean): Observable<SearchBpmnFilesResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.BPMN_API_URL + "/processfiles/search";
        const request: any = { startIndex: startIndex, count: count, takeLatest: takeLatest };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchBpmnFilesResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    get(id: string): Observable<BpmnFile> {
        let headers = new HttpHeaders();
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.BPMN_API_URL + "/processfiles/" + id;
        return this.http.get<BpmnFile>(targetUrl, { headers: headers });
    }
}