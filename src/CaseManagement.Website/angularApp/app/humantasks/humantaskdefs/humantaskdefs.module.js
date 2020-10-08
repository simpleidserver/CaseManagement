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
import { TranslateFieldPipe } from '@app/infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { HumanTaskDefsRoutes } from './humantaskdefs.routes';
import { ViewHumanTaskDefInfoComponent } from './view/info/info.component';
import { ViewHumanTaskDefRenderingComponent } from './view/rendering/rendering.component';
import { ViewHumanTaskDef } from './view/view.component';
var HumanTaskDefsModule = (function () {
    function HumanTaskDefsModule() {
    }
    HumanTaskDefsModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                NgxChartsModule,
                MonacoEditorModule.forRoot(),
                FormsModule,
                HttpClientModule,
                HumanTaskDefsRoutes,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [],
            declarations: [
                ViewHumanTaskDef,
                ViewHumanTaskDefRenderingComponent,
                ViewHumanTaskDefInfoComponent,
                TranslateFieldPipe
            ],
            exports: []
        })
    ], HumanTaskDefsModule);
    return HumanTaskDefsModule;
}());
export { HumanTaskDefsModule };
//# sourceMappingURL=humantaskdefs.module.js.map