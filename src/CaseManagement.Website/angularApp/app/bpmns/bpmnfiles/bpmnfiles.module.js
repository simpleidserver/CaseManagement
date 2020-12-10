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
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { BpmnFilesRoutes } from './bpmnfiles.routes';
import { ListBpmnFilesComponent } from './list/list.component';
import { ViewBpmnFileInformationComponent } from './view/information/information.component';
import { ListBpmnInstancesComponent } from './view/instances/instances.component';
import { ViewBpmnFileUIEditorComponent } from './view/uieditor/uieditor.component';
import { ViewBpmnFileComponent } from './view/view.component';
import { ViewBpmnFileXMLEditorComponent } from './view/xmleditor/xmleditor.component';
var BpmnFilesModule = (function () {
    function BpmnFilesModule() {
    }
    BpmnFilesModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                NgxChartsModule,
                MonacoEditorModule.forRoot(),
                FormsModule,
                HttpClientModule,
                BpmnFilesRoutes,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [],
            declarations: [ListBpmnFilesComponent, ViewBpmnFileComponent, ViewBpmnFileInformationComponent, ViewBpmnFileXMLEditorComponent, ViewBpmnFileUIEditorComponent, ListBpmnInstancesComponent],
            exports: [ListBpmnFilesComponent]
        })
    ], BpmnFilesModule);
    return BpmnFilesModule;
}());
export { BpmnFilesModule };
//# sourceMappingURL=bpmnfiles.module.js.map