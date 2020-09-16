import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CaseFilesRoutes } from './casefiles.routes';
import { HistoryCaseFileComponent } from './history/history.component';
import { AddCaseFileDialog } from './list/add-case-file-dialog';
import { ListCaseFilesComponent } from './list/list.component';
import { ViewCaseFilesComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        CaseFilesRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [ AddCaseFileDialog ],
    declarations: [ListCaseFilesComponent, AddCaseFileDialog, ViewCaseFilesComponent, HistoryCaseFileComponent ],
    exports: [ListCaseFilesComponent, ViewCaseFilesComponent ]
})

export class CaseFilesModule { }
