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
import { CmmnFilesRoutes } from './cmmnfiles.routes';
import { AddCmmnFileDialog } from './list/add-cmmn-file-dialog';
import { ListCmmnFilesComponent } from './list/list.component';
import { ViewCmmnFileComponent } from './view/view.component';
import { ViewCmmnFileInformationComponent } from './view/information/information.component';
import { ViewCmmnFileXmlEditorComponent } from './view/xmleditor/xmleditor.component';
import { ViewCmmnFileUIEditorComponent } from './view/uieditor/uieditor.component';
import { ListCmmnPlansComponent } from './view/plans/plans.component';
var CmmnFilesModule = (function () {
    function CmmnFilesModule() {
    }
    CmmnFilesModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                NgxChartsModule,
                MonacoEditorModule.forRoot(),
                FormsModule,
                HttpClientModule,
                CmmnFilesRoutes,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [AddCmmnFileDialog],
            declarations: [
                ListCmmnFilesComponent,
                ViewCmmnFileUIEditorComponent,
                AddCmmnFileDialog,
                ViewCmmnFileComponent,
                ViewCmmnFileInformationComponent,
                ViewCmmnFileXmlEditorComponent,
                ListCmmnPlansComponent
            ],
            exports: []
        })
    ], CmmnFilesModule);
    return CmmnFilesModule;
}());
export { CmmnFilesModule };
//# sourceMappingURL=cmmnfiles.module.js.map