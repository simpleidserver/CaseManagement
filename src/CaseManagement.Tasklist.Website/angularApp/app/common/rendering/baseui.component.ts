import { ViewContainerRef, OnInit, EventEmitter } from "@angular/core";

export class BaseUIComponent implements OnInit {
    option: any;
    uiOption: any;
    parent: ViewContainerRef;
    onInitialized = new EventEmitter();

    openDialog() {

    }

    ngOnInit(): void {
        this.onInitialized.emit();
        this.init();
    }

    init() {

    }
}