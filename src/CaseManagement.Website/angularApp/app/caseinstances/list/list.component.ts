import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'list-case-instances',
    templateUrl: './list.component.html',  
    styleUrls: ['./list.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ListCaseInstancesComponent implements OnInit, OnDestroy {
    constructor() { }

    ngOnInit() {
    }

    ngOnDestroy() {
    }
}