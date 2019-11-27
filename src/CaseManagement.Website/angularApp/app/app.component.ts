import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
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
        '../../node_modules/leaflet-search/dist/leaflet-search.src.css'
    ],
    encapsulation: ViewEncapsulation.None
})

export class AppComponent implements OnInit {
    breadCrumbList: Array<BreadCrumbItem> = [];
    sessionCheckEventListener: EventListener;
    sessionCheckTimer: any;
    iFrameName: string;

    constructor(translate: TranslateService, private router: Router) {
        translate.setDefaultLang('fr');
        translate.use('fr');
    }

    ngOnInit(): void {
        this.listenRouting();
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
