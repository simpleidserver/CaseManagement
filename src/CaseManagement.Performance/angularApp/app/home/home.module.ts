import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { HomeComponent } from './components/home.component';
import { HomeRoutes } from './home.routes';

@NgModule({
    imports: [
        CommonModule,
        NgxChartsModule,
        FormsModule,
        HttpClientModule,
        HomeRoutes,
        MaterialModule,
        SharedModule
    ],

    declarations: [
        HomeComponent
    ],

    exports: [
        HomeComponent
    ],

    providers: [
    ]
})

export class HomeModule { }
