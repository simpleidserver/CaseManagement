var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
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
var CasePlansModule = (function () {
    function CasePlansModule() {
    }
    CasePlansModule = __decorate([
        NgModule({
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
            entryComponents: [],
            declarations: [ListCasePlansComponent, ViewCaseDefinitionComponent, HistoryCasePlanComponent],
            exports: [ListCasePlansComponent, ViewCaseDefinitionComponent, HistoryCasePlanComponent],
            providers: [CasePlanService]
        })
    ], CasePlansModule);
    return CasePlansModule;
}());
export { CasePlansModule };
//# sourceMappingURL=caseplans.module.js.map