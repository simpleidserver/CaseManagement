import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CountResult } from '../models/count-result.model';

@Injectable()
export class CaseFilesService {
    constructor(private http: HttpClient) { }
    count(): Observable<CountResult> {
        let headers = new HttpHeaders();
        headers = headers.set('Accept', 'application/json');
        let targetUrl = process.env.API_URL + "/case-files/count";
        return this.http.get(targetUrl, { headers: headers }).pipe(map((res: any) => {
            return CountResult.fromJson(res);
        }));
    }
}