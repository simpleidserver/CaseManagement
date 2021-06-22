var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { NgModule } from '@angular/core';
import { DynamicComponent } from './dynamic.component';
import { TxtComponent } from './txt/txt.component';
import { ColumnComponent } from './column/column.component';
import { RowComponent } from './row/row.component';
import { HeaderComponent } from './header/header.component';
import { SelectComponent } from './select/select.component';
import { ContainerComponent } from './container/container.component';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
var RenderingModule = (function () {
    function RenderingModule() {
    }
    RenderingModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [
                ContainerComponent,
                TxtComponent,
                SelectComponent,
            ],
            declarations: [
                ContainerComponent,
                HeaderComponent,
                SelectComponent,
                ColumnComponent,
                RowComponent,
                TxtComponent,
                DynamicComponent
            ],
            exports: [
                ContainerComponent,
                HeaderComponent,
                SelectComponent,
                ColumnComponent,
                RowComponent,
                TxtComponent,
                DynamicComponent
            ]
        })
    ], RenderingModule);
    return RenderingModule;
}());
export { RenderingModule };
//# sourceMappingURL=rendering.module.js.map