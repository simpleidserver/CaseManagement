import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-navigation',
    templateUrl: 'navigation.component.html',
    styleUrls: ['./navigation.component.scss']
})

export class NavigationComponent implements OnInit {
    constructor(private translateService: TranslateService) { }

    chooseLanguage(lng: string) {
        this.translateService.use(lng);
    }

    ngOnInit() {

    }
}
