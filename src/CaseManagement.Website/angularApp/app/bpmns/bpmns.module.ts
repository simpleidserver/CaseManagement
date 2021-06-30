import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { BpmnsComponent } from './bpmns.component';
import { BpmnsRoutes } from './bpmns.routes';
import { ListDelegateConfigurationComponent } from './listdelegates/listdelegates.component';
import { ListBpmnFilesComponent } from './listfiles/listfiles.component';
import { ViewDelegateConfigurationComponent } from './viewdelegate/viewdelegate.component';
import { ViewXmlDialog } from './viewfile/view-xml-dialog';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';
import { ViewMessageDialog } from './viewinstance/view-message-dialog';
import { ViewBpmnInstanceComponent } from './viewinstance/view.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        BpmnsRoutes,
        MonacoEditorModule.forRoot()
    ],
    entryComponents: [ViewXmlDialog, ViewMessageDialog ],
    declarations: [
        BpmnsComponent,
        ListBpmnFilesComponent,
        ViewBpmnFileComponent,
        ViewXmlDialog,
        ViewMessageDialog,
        ViewBpmnInstanceComponent,
        ListDelegateConfigurationComponent,
        ViewDelegateConfigurationComponent
    ],
    providers: [ ]
})

export class BpmnsModule { }