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
import { HomeEffects } from './components/home-effects';
import * as fromHomeReducer from './components/home-reducer';
import { HomeComponent } from './components/home.component';
import { HomeRoutes } from './home.routes';
import { CaseDefinitionsService } from './services/casedefinitions.service';
import { CaseFilesService } from './services/casefiles.service';
import { StatisticService } from './services/statistic.service';
var HomeModule = (function () {
    function HomeModule() {
    }
    HomeModule = __decorate([
        NgModule({
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
    ], HomeModule);
    return HomeModule;
}());
export { HomeModule };
//# sourceMappingURL=home.module.js.map