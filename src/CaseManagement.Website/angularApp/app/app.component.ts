import { DOCUMENT } from '@angular/common';
import { Component, Inject, NgZone, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NullValidationHandler, OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
interface BreadCrumbItem {
    name: string,
    index: number,
    path: string,
    isLast: boolean 
}

@Component({
    selector: 'app-component',
    templateUrl: './app.component.html',
    styleUrls: [
        './app.component.scss',
        '../../node_modules/leaflet/dist/leaflet.css',
        '../../node_modules/leaflet-search/dist/leaflet-search.src.css',
        '../../node_modules/cmmn-js/dist/assets/diagram-js.css',
        '../../node_modules/cmmn-js/dist/assets/cmmn-font/css/cmmn.css',
        '../../node_modules/cmmn-js/dist/assets/cmmn-font/css/cmmn-codes.css',
        '../../node_modules/cmmn-js/dist/assets/cmmn-font/css/cmmn-embedded.css'
    ],
    encapsulation: ViewEncapsulation.None
})

export class AppComponent implements OnInit {
    breadCrumbList: Array<BreadCrumbItem> = [];
    sessionCheckEventListener: EventListener;
    sessionCheckTimer: any;
    iFrameName: string;

    constructor(translate: TranslateService, private router: Router, private oauthService: OAuthService, @Inject(DOCUMENT) private document : any, private ngZone : NgZone, private storage : OAuthStorage) {
        translate.setDefaultLang('fr');
        translate.use('fr');
        this.iFrameName = "casemanagement-idserver";
        this.configureWithNewConfigApi();        
    }

    ngOnInit(): void {
        this.listenRouting();
    }

    private configureWithNewConfigApi() {
        authConfig.redirectUri = this.document.location.origin;
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new NullValidationHandler();
        this.oauthService.loadDiscoveryDocument().then((d: any) => {
            let issuer = d.info.discoveryDocument.issuer;
            let checkSessionIframe = d.info.discoveryDocument.check_session_iframe;
            this.initSessionCheck(issuer.toLowerCase(), checkSessionIframe.toLowerCase());
            return this.oauthService.tryLogin();
        });
    }

    private initSessionCheck(issuer: string, checkSessionIFrame: string): void {
        const existingIframe = document.getElementById(this.iFrameName);
        if (existingIframe) {
            document.body.removeChild(existingIframe);
        }

        const iframe = document.createElement('iframe');
        iframe.id = this.iFrameName;

        this.setupSessionCheckEventListener(issuer);

        const url = checkSessionIFrame;
        iframe.setAttribute('src', url);
        iframe.style.display = 'none';
        document.body.appendChild(iframe);
        this.startSessionCheckTimer(issuer);
    }

    private startSessionCheckTimer(issuer: string): void {
        this.stopSessionCheckTimer();
        this.ngZone.runOutsideAngular(() => {
            this.sessionCheckTimer = setInterval(
                this.checkSession.bind(this, issuer),
                3000
            );
        });
    }

    private checkSession(issuer: string): void {
        const iframe: any = document.getElementById(this.iFrameName);
        if (!iframe) {
            console.log('checkSession did not find iframe');
            return;
        }

        const sessionState = this.storage.getItem('session_state');
        if (!sessionState) {
            this.stopSessionCheckTimer();
        }

        const message = this.oauthService.clientId + ' ' + sessionState;
        iframe.contentWindow.postMessage(message, issuer);
    }

    private setupSessionCheckEventListener(issuer: string): void {
        this.removeSessionCheckEventListener();
        this.sessionCheckEventListener = (e: MessageEvent) => {
            const origin = e.origin.toLowerCase();
            if (!issuer.startsWith(origin)) {
                console.log('sessionCheckEventListener', 'wrong origin', origin, 'expected', issuer);
            }

            switch (e.data) {
                case 'changed':
                case 'error':
                    this.stopSessionCheckTimer();
                    this.oauthService.logOut(true);
                    break;
            }
        };

        this.ngZone.runOutsideAngular(() => {
            window.addEventListener('message', this.sessionCheckEventListener);
        });
    }

    private stopSessionCheckTimer(): void {
        if (this.sessionCheckTimer) {
            clearInterval(this.sessionCheckTimer);
            this.sessionCheckTimer = null;
        }
    }

    private removeSessionCheckEventListener(): void {
        if (this.sessionCheckEventListener) {
            window.removeEventListener('message', this.sessionCheckEventListener);
            this.sessionCheckEventListener = null;
        }
    }

    private listenRouting() {
        var self = this;
        let routerUrl: string;
        let path: string;
        let routerList: Array<any>;
        this.router.events.subscribe((router: any) => {
            routerUrl = router.urlAfterRedirects;
            if (!routerUrl || typeof routerUrl !== 'string') {
                return;
            }

            path = '';
            self.breadCrumbList.length = 0;
            if (routerUrl.includes('filter')) {
                return;
            }

            routerList = routerUrl.slice(1).split('/');
            routerList.forEach(function (router, index) {
                path += '/' + decodeURIComponent(router);
                self.breadCrumbList.push({
                    name: self.cleanUri(decodeURIComponent(router)),
                    index: index,
                    path: path,
                    isLast: index === routerList.length - 1
                });
            });
        });
    }

    private cleanUri(uri :string) : string {
        return uri.replace(/(\?.*)|(#.*)/g, "");
    }
}
