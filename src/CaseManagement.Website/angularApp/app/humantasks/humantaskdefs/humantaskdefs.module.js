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
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { HumanTaskDefsRoutes } from './humantaskdefs.routes';
import { ViewHumanTaskDef } from './view/view.component';
import { AddHumanTaskDefDialog } from './list/add-humantaskdef-dialog.component';
import { ListHumanTaskDef } from './list/list.component';
import { CreateHumanTaskInstanceDialog } from './view/create-humantaskinstance-dialog.component';
import { CreateHumanTaskInstanceComponent } from './view/common/createhumantaskinstance/createhumantaskinstance.component';
var HumanTaskDefsModule = (function () {
    function HumanTaskDefsModule() {
    }
    HumanTaskDefsModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                FormsModule,
                HttpClientModule,
                HumanTaskDefsRoutes,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [AddHumanTaskDefDialog, CreateHumanTaskInstanceDialog],
            declarations: [
                ViewHumanTaskDef,
                ListHumanTaskDef,
                AddHumanTaskDefDialog,
                CreateHumanTaskInstanceDialog,
                CreateHumanTaskInstanceComponent
            ],
            exports: []
        })
    ], HumanTaskDefsModule);
    return HumanTaskDefsModule;
}());
export { HumanTaskDefsModule };
//# sourceMappingURL=humantaskdefs.module.js.map