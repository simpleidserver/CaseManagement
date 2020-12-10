var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { ActivityStatesComponent } from './activitystates/activitystates.component';
import { ViewBpmnInstanceComponent } from './view.component';
import { ViewBpmnInstanceRoutes } from './view.routes';
import { ViewExecutionPathComponent } from './viewexecutionpath.component';
import { ViewExecutionPointerComponent } from './viewpointer.component';
import { IncomingTokensComponent } from './incomingtokens/incomingtokens.component';
import { OutgoingTokensComponent } from './outgoingtokens/outgoingtokens.component';
var ViewBpmnInstanceModule = (function () {
    function ViewBpmnInstanceModule() {
    }
    ViewBpmnInstanceModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                NgxChartsModule,
                MonacoEditorModule.forRoot(),
                FormsModule,
                HttpClientModule,
                ViewBpmnInstanceRoutes,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [],
            declarations: [ActivityStatesComponent, ViewBpmnInstanceComponent, ViewExecutionPathComponent, ViewExecutionPointerComponent, IncomingTokensComponent, OutgoingTokensComponent]
        })
    ], ViewBpmnInstanceModule);
    return ViewBpmnInstanceModule;
}());
export { ViewBpmnInstanceModule };
//# sourceMappingURL=view.module.js.map