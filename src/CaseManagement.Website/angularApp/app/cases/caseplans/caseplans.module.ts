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
import { CasePlansRoutes } from './caseplans.routes';
import { CasePlanEffects } from './effects/caseplan';
import { HistoryCasePlanComponent } from './history/history.component';
import { ListCasePlansComponent } from './list/list.component';
import * as reducers from './reducers';
import { CasePlanService } from './services/caseplan.service';
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
        EffectsModule.forRoot([CasePlanEffects]),
        StoreModule.forRoot(reducers.appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ ],
    declarations: [ListCasePlansComponent, ViewCaseDefinitionComponent, HistoryCasePlanComponent],
    exports: [ListCasePlansComponent, ViewCaseDefinitionComponent, HistoryCasePlanComponent],
    providers: [CasePlanService]
})

export class CasePlansModule { }
