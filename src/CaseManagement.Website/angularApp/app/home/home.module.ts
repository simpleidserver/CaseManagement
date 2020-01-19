import { CommonModule, DatePipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { HomeEffects } from './components/home-effects';
import * as fromHomeReducer from './components/home-reducer';
import { HomeComponent } from './components/home.component';
import { HomeRoutes } from './home.routes';
import { StatisticService } from './services/statistic.service';
import { CaseFilesService } from '../casedefinitions/services/casefiles.service';
import { CaseDefinitionsService } from '../casedefinitions/services/casedefinitions.service';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        HomeRoutes,
        MaterialModule,
        SharedModule,
        EffectsModule.forRoot([HomeEffects]),
        StoreModule.forRoot({
            statistic: fromHomeReducer.statisticReducer,
            weekStatistics: fromHomeReducer.weekStatisticsReducer,
            monthStatistics: fromHomeReducer.monthStatisticsReducer,
            deployed: fromHomeReducer.deployedReducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],

    declarations: [
        HomeComponent
    ],

    exports: [
        HomeComponent
    ],

    providers: [
        StatisticService,
        CaseFilesService,
        CaseDefinitionsService,
        DatePipe
    ]
})

export class HomeModule { }
