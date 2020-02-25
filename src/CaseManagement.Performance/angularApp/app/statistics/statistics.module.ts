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
import { StatisticsEffects } from './components/statistics-effects';
import * as fromStatisticsReducer from './components/statistics-reducer';
import { StatisticsComponent } from './components/statistics.component';
import { StatisticsRoutes } from './statistics.routes';
import { StatisticService } from './services/statistic.service';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        StatisticsRoutes,
        MaterialModule,
        SharedModule,
        EffectsModule.forRoot([StatisticsEffects]),
        StoreModule.forRoot({
            statistic: fromStatisticsReducer.statisticReducer,
            weekStatistics: fromStatisticsReducer.weekStatisticsReducer,
            monthStatistics: fromStatisticsReducer.monthStatisticsReducer,
            deployed: fromStatisticsReducer.deployedReducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],

    declarations: [
        StatisticsComponent
    ],

    exports: [
        StatisticsComponent
    ],

    providers: [
        StatisticService,
        DatePipe
    ]
})

export class StatisticsModule { }
