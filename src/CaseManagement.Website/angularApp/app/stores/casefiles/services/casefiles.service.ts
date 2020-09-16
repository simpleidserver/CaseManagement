import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CaseFile } from '../models/case-file.model';
import { SearchCaseFilesResult } from '../models/search-case-files-result.model';

@Injectable()
export class CaseFilesService {
    constructor(private http: HttpClient, private oauthService: OAuthService) { }

    search(startIndex: number, count: number, order: string, direction: string, text: string, caseFileId : string, takeLatest : boolean): Observable<SearchCaseFilesResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        const targetUrl = process.env.API_URL + "/case-files/search";
        const request: any = { startIndex: startIndex, count: count, takeLatest: takeLatest };
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

        return this.http.post<SearchCaseFilesResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }

    get(id: string): Observable<CaseFile> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-files/" + id;
        return this.http.get<CaseFile>(targetUrl, { headers: headers });
    }

    add(name: string, description: string) : Observable<string> {
        const request = JSON.stringify({ name: name, description: description});
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-files";
        return this.http.post(targetUrl, request, { headers: headers }).pipe(map((res: any) => {
            return res['id'];
        }));
    }

    update(id: string, name: string, description: string, payload: string): Observable<any> {
        const request = JSON.stringify({ name: name, description: description, payload: payload });
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-files/" + id;
        return this.http.put(targetUrl, request, { headers: headers });
    }

    publish(id: string) : Observable<string> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        let targetUrl = process.env.API_URL + "/case-files/" + id + "/publish";
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return res['id'];
        }));
    }
}