var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
import { DOCUMENT } from '@angular/common';
import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { Router } from '@angular/router';
var AppComponent = (function () {
    function AppComponent(route, translate, oauthService, document) {
        this.route = route;
        this.oauthService = oauthService;
        this.document = document;
        translate.setDefaultLang('fr');
        translate.use('fr');
        this.configureAuth();
    }
    AppComponent.prototype.configureAuth = function () {
        authConfig.redirectUri = this.document.location.origin;
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new JwksValidationHandler();
        var self = this;
        this.oauthService.loadDiscoveryDocumentAndTryLogin();
        this.sessionCheckTimer = setInterval(function () {
            if (!self.oauthService.hasValidIdToken()) {
                self.oauthService.logOut();
                self.route.navigate(["/"]);
            }
        }, 3000);
    };
    AppComponent = __decorate([
        Component({
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
        }),
        __param(3, Inject(DOCUMENT)),
        __metadata("design:paramtypes", [Router, TranslateService, OAuthService, Object])
    ], AppComponent);
    return AppComponent;
}());
export { AppComponent };
//# sourceMappingURL=app.component.js.map