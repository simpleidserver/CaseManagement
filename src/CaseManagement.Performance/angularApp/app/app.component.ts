import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';

@Component({
    selector: 'app-component',
    templateUrl: './app.component.html',
    styleUrls: [
        './app.component.scss',
        '../../node_modules/leaflet/dist/leaflet.css',
        '../../node_modules/leaflet-search/dist/leaflet-search.src.css'
    ],
    encapsulation: ViewEncapsulation.None
})

export class AppComponent {
    sessionCheckTimer: any;

    constructor(translate: TranslateService, private router: Router, private oauthService: OAuthService) {
        translate.setDefaultLang('fr');
        translate.use('fr');
        this.configureAuth();
    }

    private configureAuth() {
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        let self = this;
        this.oauthService.loadDiscoveryDocumentAndTryLogin({
            disableOAuth2StateCheck: true
        });
        this.sessionCheckTimer = setInterval(function () {
            if (!self.oauthService.hasValidIdToken()) {
                self.oauthService.logOut();
                self.router.navigate(["/"]);
            }
        }, 3000);
    }
}
