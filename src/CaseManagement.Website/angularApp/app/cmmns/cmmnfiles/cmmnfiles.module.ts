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

@NgModule({
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
    entryComponents: [ AddCmmnFileDialog ],
    declarations: [
        ListCmmnFilesComponent,
        ViewCmmnFileUIEditorComponent,
        AddCmmnFileDialog,
        ViewCmmnFileComponent,
        ViewCmmnFileInformationComponent,
        ViewCmmnFileXmlEditorComponent,
        ListCmmnPlansComponent
    ],
    exports: [ ]
})

export class CmmnFilesModule { }
