import { ViewContainerRef, OnInit, EventEmitter } from "@angular/core";
import { FormGroup } from "@angular/forms";

export class BaseUIComponent implements OnInit {
    option: any;
    uiOption: any;
    form: FormGroup;
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