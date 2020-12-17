import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { CmmnsComponent } from './cmmns.component';
import { CmmnsRoutes } from './cmmns.routes';
import { ListCmmnFilesComponent } from './listfiles/listfiles.component';
import { ViewXmlDialog } from './viewfile/view-xml-dialog';
import { ViewCmmnFileComponent } from './viewfile/viewfile.component';
import { ViewMessageDialog } from './viewinstance/view-message-dialog';
import { ViewCmmnInstanceComponent } from './viewinstance/viewinstance.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        CmmnsRoutes,
        MonacoEditorModule.forRoot()
    ],
    entryComponents: [ ViewXmlDialog, ViewMessageDialog ],
    declarations: [
        CmmnsComponent,
        ListCmmnFilesComponent,
        ViewCmmnFileComponent,
        ViewCmmnInstanceComponent,
        ViewXmlDialog,
        ViewMessageDialog
    ],
    providers: [ ]
})

export class CmmnsModule { }