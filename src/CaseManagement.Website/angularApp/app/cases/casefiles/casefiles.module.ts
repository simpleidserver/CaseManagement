import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CaseFilesRoutes } from './casefiles.routes';
import { CaseFilesEffects } from './effects/case-files';
import { AddCaseFileDialog } from './list/add-case-file-dialog';
import { ListCaseFilesComponent } from './list/list.component';
import * as reducers from './reducers';
import { CaseFilesService } from './services/casefiles.service';
import { ViewCaseFilesComponent } from './view/view.component';
import { MonacoEditorModule } from 'ngx-monaco-editor';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        CaseFilesRoutes,
        MaterialModule,
        SharedModule,
        EffectsModule.forRoot([CaseFilesEffects]),
        StoreModule.forRoot(reducers.appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ AddCaseFileDialog ],
    declarations: [ListCaseFilesComponent, AddCaseFileDialog, ViewCaseFilesComponent ],
    exports: [ListCaseFilesComponent, ViewCaseFilesComponent ],
    providers: [ CaseFilesService ]
})

export class CaseFilesModule { }
