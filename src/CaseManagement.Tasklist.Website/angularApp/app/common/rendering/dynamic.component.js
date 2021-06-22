var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ComponentFactoryResolver, Input, ViewChild, ViewContainerRef } from "@angular/core";
import { ColumnComponent } from "./column/column.component";
import { ContainerComponent } from "./container/container.component";
import { RowComponent } from "./row/row.component";
import { SelectComponent } from "./select/select.component";
import { TxtComponent } from "./txt/txt.component";
import { HeaderComponent } from "./header/header.component";
var DynamicComponent = (function () {
    function DynamicComponent(compFactoryResolver) {
        this.compFactoryResolver = compFactoryResolver;
        this._uiOption = {
            editMode: false
        };
        this._dic = {
            'row': RowComponent,
            'column': ColumnComponent,
            'txt': TxtComponent,
            'select': SelectComponent,
            'container': ContainerComponent,
            'header': HeaderComponent
        };
        this.baseUIComponent = null;
        this.parentOption = null;
        this.isSelected = false;
        this.title = null;
    }
    Object.defineProperty(DynamicComponent.prototype, "option", {
        get: function () {
            return this._option;
        },
        set: function (val) {
            if (!val) {
                return;
            }
            this._option = val;
            this.refresh();
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(DynamicComponent.prototype, "uiOption", {
        get: function () {
            return this._uiOption;
        },
        set: function (val) {
            if (!val) {
                return;
            }
            this._uiOption = val;
            this.refresh();
        },
        enumerable: false,
        configurable: true
    });
    DynamicComponent.prototype.ngOnInit = function () {
    };
    DynamicComponent.prototype.ngOnDestroy = function () {
        if (!this.componentRef) {
            return;
        }
        this.componentRef.destroy();
        this.componentRef = null;
    };
    DynamicComponent.prototype.clickComponent = function (evt) {
        if (!this._uiOption.editMode) {
            return;
        }
        evt.preventDefault();
        evt.stopPropagation();
        this.isSelected = !this.isSelected;
    };
    DynamicComponent.prototype.dropElt = function (evt) {
        if (!this._uiOption.editMode) {
            return;
        }
        evt.preventDefault();
        evt.stopPropagation();
        if (!this.option.children) {
            return;
        }
        var json = JSON.parse(evt.dataTransfer.getData('json'));
        this.option.children.push(json);
    };
    DynamicComponent.prototype.openSettings = function (evt) {
        if (!this._uiOption.editMode) {
            return;
        }
        evt.preventDefault();
        evt.stopPropagation();
        if (!this.baseUIComponent) {
            return;
        }
        this.baseUIComponent.openDialog();
    };
    DynamicComponent.prototype.remove = function (evt) {
        if (!this._uiOption.editMode) {
            return;
        }
        evt.preventDefault();
        evt.stopPropagation();
        if (!this.baseUIComponent || !this.parentOption || !this.parentOption.children) {
            return;
        }
        var self = this;
        var filtered = this.parentOption.children.filter(function (r) {
            return self.baseUIComponent.option.id === r.id;
        });
        if (filtered.length !== 1) {
            return;
        }
        var index = this.parentOption.children.indexOf(filtered[0]);
        this.parentOption.children.splice(index, 1);
    };
    DynamicComponent.prototype.refresh = function () {
        var _this = this;
        if (!this.option) {
            return;
        }
        this.container.clear();
        var type = this._dic[this.option.type];
        if (!type) {
            return;
        }
        var factory = this.compFactoryResolver.resolveComponentFactory(type);
        this.componentRef = this.container.createComponent(factory);
        this.baseUIComponent = this.componentRef.instance;
        this.baseUIComponent.option = this.option;
        this.baseUIComponent.onInitialized.subscribe(function () {
            _this.baseUIComponent.uiOption = _this.uiOption;
            _this.baseUIComponent.parent = _this.parent;
        });
        this.title = this.option.type;
    };
    __decorate([
        ViewChild('container', { read: ViewContainerRef }),
        __metadata("design:type", ViewContainerRef)
    ], DynamicComponent.prototype, "container", void 0);
    __decorate([
        ViewChild('parent', { read: ViewContainerRef }),
        __metadata("design:type", ViewContainerRef)
    ], DynamicComponent.prototype, "parent", void 0);
    __decorate([
        Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], DynamicComponent.prototype, "option", null);
    __decorate([
        Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], DynamicComponent.prototype, "uiOption", null);
    __decorate([
        Input(),
        __metadata("design:type", Object)
    ], DynamicComponent.prototype, "parentOption", void 0);
    DynamicComponent = __decorate([
        Component({
            selector: 'dynamic-component',
            templateUrl: 'dynamic.component.html',
            styleUrls: ['./dynamic.component.scss']
        }),
        __metadata("design:paramtypes", [ComponentFactoryResolver])
    ], DynamicComponent);
    return DynamicComponent;
}());
export { DynamicComponent };
//# sourceMappingURL=dynamic.component.js.map