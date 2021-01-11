import { Component, ComponentFactoryResolver, ComponentRef, Input, OnDestroy, OnInit, ViewChild, ViewContainerRef } from "@angular/core";
import { BaseUIComponent } from "./baseui.component";
import { ColumnComponent } from "./column/column.component";
import { ContainerComponent } from "./container/container.component";
import { RowComponent } from "./row/row.component";
import { SelectComponent } from "./select/select.component";
import { TxtComponent } from "./txt/txt.component";
import { HeaderComponent } from "./header/header.component";

@Component({
    selector: 'dynamic-component',
    templateUrl: 'dynamic.component.html',
    styleUrls: ['./dynamic.component.scss']

})
export class DynamicComponent implements OnInit, OnDestroy {
    private componentRef: ComponentRef<{}>;
    private _dic: any = {
        'row': RowComponent,
        'column': ColumnComponent,
        'txt': TxtComponent,
        'select': SelectComponent,
        'container': ContainerComponent,
        'header': HeaderComponent
    };
    @ViewChild('container', { read: ViewContainerRef }) container: ViewContainerRef;
    @ViewChild('parent', { read: ViewContainerRef }) parent: ViewContainerRef;
    baseUIComponent: BaseUIComponent = null;
    @Input() option: any = null;
    @Input() parentOption: any = null;

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

    openSettings(evt: any) {
        evt.preventDefault();
        evt.stopPropagation();
        if (!this.baseUIComponent) {
            return;
        }

        this.baseUIComponent.openDialog();
    }

    remove(evt: any) {
        evt.preventDefault();
        evt.stopPropagation();
        if (!this.baseUIComponent || !this.parentOption || !this.parentOption.children) {
            return;
        }

        const self = this;
        const filtered = this.parentOption.children.filter((r: any) => {
            return self.baseUIComponent.option.id === r.id;
        });
        if (filtered.length !== 1) {
            return;
        }

        const index = this.parentOption.children.indexOf(filtered[0]);
        this.parentOption.children.splice(index, 1);
    }

    private refresh() {
        if (!this.option || !this.option.type) {
            return;
        }

        const type = this._dic[this.option.type];
        const factory = this.compFactoryResolver.resolveComponentFactory(type);
        this.componentRef = this.container.createComponent(factory);
        this.baseUIComponent = this.componentRef.instance as BaseUIComponent;
        this.baseUIComponent .option = this.option;
        this.baseUIComponent .parent = this.parent;
        this.title = this.option.type;
    }
}