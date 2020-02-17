import { DOCUMENT } from '@angular/common';
import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { Router } from '@angular/router';

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

export class AppComponent {
    sessionCheckTimer: any;

    constructor(private route: Router, translate: TranslateService, private oauthService: OAuthService , @Inject(DOCUMENT) private document : any) {
        translate.setDefaultLang('fr');
        translate.use('fr');
        this.configureAuth();        
    }

    private configureAuth() {
        authConfig.redirectUri = this.document.location.origin;
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        let self = this;
        this.oauthService.loadDiscoveryDocumentAndTryLogin();
        this.sessionCheckTimer = setInterval(function () {
            if (!self.oauthService.hasValidIdToken()) {
                self.oauthService.logOut();
                self.route.navigate(["/"]);
            }            
        }, 3000);
    }
}
