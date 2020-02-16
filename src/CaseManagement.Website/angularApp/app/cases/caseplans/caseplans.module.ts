import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CaseFilesEffects } from '../casefiles/effects/case-files';
import { CaseFilesService } from '../casefiles/services/casefiles.service';
import { CasePlansRoutes } from './caseplans.routes';
import { CaseActivationsEffects } from './effects/case-activations';
import { CaseFormInstancesEffects } from './effects/case-form-instances';
import { CaseInstancesEffects } from './effects/case-instances';
import { CasePlansEffects } from './effects/case-plans';
import { ListCasePlansComponent } from './list/list.component';
import * as reducers from './reducers';
import { CaseActivationsService } from './services/caseactivations.service';
import { CaseFormInstancesService } from './services/caseforminstances.service';
import { CaseInstancesService } from './services/caseinstances.service';
import { CasePlansService } from './services/caseplans.service';
import { ViewCaseDefinitionComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        MonacoEditorModule.forRoot(),
        FormsModule,
        HttpClientModule,
        CasePlansRoutes,
        MaterialModule,
        SharedModule,
        EffectsModule.forRoot([CasePlansEffects, CaseActivationsEffects, CaseFormInstancesEffects, CaseInstancesEffects, CaseFilesEffects]),
        StoreModule.forRoot(reducers.appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ ],
    declarations: [ListCasePlansComponent, ViewCaseDefinitionComponent],
    exports: [ListCasePlansComponent, ViewCaseDefinitionComponent],
    providers: [CasePlansService, CaseActivationsService, CaseFormInstancesService, CaseInstancesService, CaseFilesService]
})

export class CasePlansModule { }
