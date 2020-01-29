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
import { ListPerformancesEffects } from './list/list-effects';
import * as fromListReducer from './list/list-reducer';
import { ListPerformanceComponent } from './list/list.component';
import { PerformancesRoutes } from './performances.routes';
import { StatisticService } from './services/statistic.service';
var PerformancesModule = (function () {
    function PerformancesModule() {
    }
    PerformancesModule = __decorate([
        NgModule({
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
    ], PerformancesModule);
    return PerformancesModule;
}());
export { PerformancesModule };
//# sourceMappingURL=performances.module.js.map