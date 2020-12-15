var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { BpmnsComponent } from './bpmns.component';
import { BpmnsRoutes } from './bpmns.routes';
import { ListBpmnFilesComponent } from './listfiles/listfiles.component';
import { ViewXmlDialog } from './viewfile/view-xml-dialog';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';
import { ViewBpmnInstanceComponent } from './viewinstance/view.component';
var BpmnsModule = (function () {
    function BpmnsModule() {
    }
    BpmnsModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                SharedModule,
                MaterialModule,
                BpmnsRoutes,
                MonacoEditorModule.forRoot()
            ],
            entryComponents: [ViewXmlDialog],
            declarations: [
                BpmnsComponent,
                ListBpmnFilesComponent,
                ViewBpmnFileComponent,
                ViewXmlDialog,
                ViewBpmnInstanceComponent
            ],
            providers: []
        })
    ], BpmnsModule);
    return BpmnsModule;
}());
export { BpmnsModule };
//# sourceMappingURL=bpmns.module.js.map