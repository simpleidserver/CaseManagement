import { Component, ComponentFactoryResolver, ComponentRef, Input, OnDestroy, OnInit, ViewChild, ViewContainerRef } from "@angular/core";
import { ColumnComponent } from "./column/column.component";
import { RowComponent } from "./row/row.component";
import { TxtComponent } from "./txt/txt.component";
import { BaseUIComponent } from "./baseui.component";
import { SelectComponent } from "./select/select.component";

@Component({
    selector: 'dynamic-component',
    templateUrl: 'dynamic.component.html',
    styleUrls: ['./dynamic.component.scss']

})
export class DynamicComponent implements OnInit, OnDestroy {
    private _option: any;
    private componentRef: ComponentRef<{}>;
    private _dic: any = {
        'row': RowComponent,
        'column': ColumnComponent,
        'txt': TxtComponent,
        'select': SelectComponent
    };
    @ViewChild('container', { read: ViewContainerRef }) container: ViewContainerRef;
    @ViewChild('parent', { read: ViewContainerRef }) parent: ViewContainerRef;
    @Input()
    get option() {
        return this._option;
    }
    set option(v: any) {
        if (!v) {
            return;
        }

        this._option = v;
    }
    isSelected: boolean = false;
    title: string = null;

    constructor(private compFactoryResolver: ComponentFactoryResolver) { }

    ngOnInit() {
        this.refresh();
    }

    ngOnDestroy() {
        if (!this.componentRef) {
            return;
        }

        this.componentRef.destroy();
        this.componentRef = null;
    }

    clickComponent(evt: any) {
        evt.preventDefault();
        evt.stopPropagation();
        this.isSelected = !this.isSelected;
    }

    dropElt(evt: any) {
        evt.preventDefault();
        evt.stopPropagation();
        if (!this.option.children) {
            return;
        }

        const json = JSON.parse(evt.dataTransfer.getData('json'));
        this.option.children.push(json);
    }

    private refresh() {
        if (!this.option || !this.option.type) {
            return;
        }

        const type = this._dic[this.option.type];
        const factory = this.compFactoryResolver.resolveComponentFactory(type);
        this.componentRef = this.container.createComponent(factory);
        const instance = this.componentRef.instance as BaseUIComponent;
        instance.option = this.option;
        instance.parent = this.parent;
        this.title = this.option.type;
    }
}