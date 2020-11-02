import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { SearchNotificationResult } from '../models/search-notification.model';

@Injectable()
export class NotificationsService {
    constructor(private http: HttpClient,
        private oauthService: OAuthService,
        private translate: TranslateService) { }

    search(startIndex: number, count: number, order: string, direction: string): Observable<SearchNotificationResult> {
        let headers = new HttpHeaders();
        const defaultLang = this.translate.currentLang;
        headers = headers.set('Accept', 'application/json');
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Authorization', 'Bearer ' + this.oauthService.getIdToken());
        headers = headers.set('Accept-Language', defaultLang);
        const targetUrl = process.env.API_URL + "/notificationinstances/.search";
        const request: any = { startIndex: startIndex, count: count };
        if (order) {
            request["orderBy"] = order;
        }

        if (direction) {
            request["order"] = direction;
        }

        return this.http.post<SearchNotificationResult>(targetUrl, JSON.stringify(request), { headers: headers });
    }
}