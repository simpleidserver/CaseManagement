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
import { Component, ViewEncapsulation, Inject, NgZone } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { authConfig } from './auth.config';
import { OAuthService, NullValidationHandler, OAuthStorage } from 'angular-oauth2-oidc';
import { DOCUMENT } from '@angular/common';
var AppComponent = (function () {
    function AppComponent(translate, router, oauthService, document, ngZone, storage) {
        this.router = router;
        this.oauthService = oauthService;
        this.document = document;
        this.ngZone = ngZone;
        this.storage = storage;
        this.breadCrumbList = [];
        translate.setDefaultLang('fr');
        translate.use('fr');
        this.iFrameName = "casemanagement-performance-idserver";
        this.configureWithNewConfigApi();
    }
    AppComponent.prototype.ngOnInit = function () {
        this.listenRouting();
    };
    AppComponent.prototype.configureWithNewConfigApi = function () {
        var _this = this;
        authConfig.redirectUri = this.document.location.origin;
        this.oauthService.configure(authConfig);
        this.oauthService.tokenValidationHandler = new NullValidationHandler();
        this.oauthService.loadDiscoveryDocument().then(function (d) {
            var issuer = d.info.discoveryDocument.issuer;
            var checkSessionIframe = d.info.discoveryDocument.check_session_iframe;
            _this.initSessionCheck(issuer.toLowerCase(), checkSessionIframe.toLowerCase());
            return _this.oauthService.tryLogin();
        });
    };
    AppComponent.prototype.initSessionCheck = function (issuer, checkSessionIFrame) {
        var existingIframe = document.getElementById(this.iFrameName);
        if (existingIframe) {
            document.body.removeChild(existingIframe);
        }
        var iframe = document.createElement('iframe');
        iframe.id = this.iFrameName;
        this.setupSessionCheckEventListener(issuer);
        var url = checkSessionIFrame;
        iframe.setAttribute('src', url);
        iframe.style.display = 'none';
        document.body.appendChild(iframe);
        this.startSessionCheckTimer(issuer);
    };
    AppComponent.prototype.startSessionCheckTimer = function (issuer) {
        var _this = this;
        this.stopSessionCheckTimer();
        this.ngZone.runOutsideAngular(function () {
            _this.sessionCheckTimer = setInterval(_this.checkSession.bind(_this, issuer), 3000);
        });
    };
    AppComponent.prototype.checkSession = function (issuer) {
        var iframe = document.getElementById(this.iFrameName);
        if (!iframe) {
            console.log('checkSession did not find iframe');
            return;
        }
        var sessionState = this.storage.getItem('session_state');
        if (!sessionState) {
            this.stopSessionCheckTimer();
        }
        var message = this.oauthService.clientId + ' ' + sessionState;
        iframe.contentWindow.postMessage(message, issuer);
    };
    AppComponent.prototype.setupSessionCheckEventListener = function (issuer) {
        var _this = this;
        this.removeSessionCheckEventListener();
        this.sessionCheckEventListener = function (e) {
            var origin = e.origin.toLowerCase();
            if (!issuer.startsWith(origin)) {
                console.log('sessionCheckEventListener', 'wrong origin', origin, 'expected', issuer);
            }
            switch (e.data) {
                case 'changed':
                case 'error':
                    _this.stopSessionCheckTimer();
                    _this.oauthService.logOut(true);
                    break;
            }
        };
        this.ngZone.runOutsideAngular(function () {
            window.addEventListener('message', _this.sessionCheckEventListener);
        });
    };
    AppComponent.prototype.stopSessionCheckTimer = function () {
        if (this.sessionCheckTimer) {
            clearInterval(this.sessionCheckTimer);
            this.sessionCheckTimer = null;
        }
    };
    AppComponent.prototype.removeSessionCheckEventListener = function () {
        if (this.sessionCheckEventListener) {
            window.removeEventListener('message', this.sessionCheckEventListener);
            this.sessionCheckEventListener = null;
        }
    };
    AppComponent.prototype.listenRouting = function () {
        var self = this;
        var routerUrl;
        var path;
        var routerList;
        this.router.events.subscribe(function (router) {
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
    };
    AppComponent.prototype.cleanUri = function (uri) {
        return uri.replace(/(\?.*)|(#.*)/g, "");
    };
    AppComponent = __decorate([
        Component({
            selector: 'app-component',
            templateUrl: './app.component.html',
            styleUrls: [
                './app.component.scss',
                '../../node_modules/leaflet/dist/leaflet.css',
                '../../node_modules/leaflet-search/dist/leaflet-search.src.css'
            ],
            encapsulation: ViewEncapsulation.None
        }),
        __param(3, Inject(DOCUMENT)),
        __metadata("design:paramtypes", [TranslateService, Router, OAuthService, Object, NgZone, OAuthStorage])
    ], AppComponent);
    return AppComponent;
}());
export { AppComponent };
//# sourceMappingURL=app.component.js.map