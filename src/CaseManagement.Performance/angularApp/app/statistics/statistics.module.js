var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
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
var StatisticsModule = (function () {
    function StatisticsModule() {
    }
    StatisticsModule = __decorate([
        NgModule({
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
    ], StatisticsModule);
    return StatisticsModule;
}());
export { StatisticsModule };
//# sourceMappingURL=statistics.module.js.map