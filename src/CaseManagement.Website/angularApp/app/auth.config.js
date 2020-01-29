var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewEncapsulation } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
var AppComponent = (function () {
    function AppComponent(translate, router) {
        this.router = router;
        this.breadCrumbList = [];
        translate.setDefaultLang('fr');
        translate.use('fr');
    }
    AppComponent.prototype.ngOnInit = function () {
        this.listenRouting();
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
        __metadata("design:paramtypes", [TranslateService, Router])
    ], AppComponent);
    return AppComponent;
}());
export { AppComponent };
//# sourceMappingURL=app.component.js.map