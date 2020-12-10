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
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { TranslateFieldPipe } from '../infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { ListTasksComponent } from './list/list.component';
import { NominateTaskDialogComponent } from './list/nominate-task-dialog.component';
import { HomeRoutes } from './tasks.routes';
import { ViewTaskComponent } from './view/view.component';
var TasksModule = (function () {
    function TasksModule() {
    }
    TasksModule = __decorate([
        NgModule({
            imports: [
                CommonModule,
                NgxChartsModule,
                FormsModule,
                HttpClientModule,
                HomeRoutes,
                MaterialModule,
                SharedModule
            ],
            entryComponents: [
                NominateTaskDialogComponent
            ],
            declarations: [
                ListTasksComponent,
                ViewTaskComponent,
                NominateTaskDialogComponent,
                TranslateFieldPipe
            ],
            exports: [
                ListTasksComponent,
                NominateTaskDialogComponent
            ],
            providers: []
        })
    ], TasksModule);
    return TasksModule;
}());
export { TasksModule };
//# sourceMappingURL=tasks.module.js.map