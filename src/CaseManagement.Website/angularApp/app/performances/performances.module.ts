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
import { ListPerformancesEffects } from './list/list-effects';
import * as fromListReducer from './list/list-reducer';
import { ListPerformanceComponent } from './list/list.component';
import { PerformancesRoutes } from './performances.routes';
import { StatisticService } from './services/statistic.service';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        PerformancesRoutes,
        MaterialModule,
        SharedModule,
        EffectsModule.forRoot([ListPerformancesEffects]),
        StoreModule.forRoot({
            performances: fromListReducer.performancesReducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],

    declarations: [
        ListPerformanceComponent
    ],

    exports: [
        ListPerformanceComponent
    ],

    providers: [
        StatisticService,
        DatePipe
    ]
})

export class PerformancesModule { }
