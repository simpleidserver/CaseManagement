import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { ListBpmnFilesComponent } from './listfiles/listfiles.component';
import { BpmnsComponent } from './bpmns.component';
import { BpmnsRoutes } from './bpmns.routes';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';
import { ViewXmlDialog } from './viewfile/view-xml-dialog';
import { MonacoEditorModule } from 'ngx-monaco-editor';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        BpmnsRoutes,
        MonacoEditorModule.forRoot()
    ],
    entryComponents: [ ViewXmlDialog ],
    declarations: [
        BpmnsComponent,
        ListBpmnFilesComponent,
        ViewBpmnFileComponent,
        ViewXmlDialog
    ],
    providers: [ ]
})

export class BpmnsModule { }