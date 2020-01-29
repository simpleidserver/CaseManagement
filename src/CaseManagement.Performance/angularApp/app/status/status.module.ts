import { NgModule } from '@angular/core';
import { UnauthorizedComponent } from './components/401/401.component';
import { NotFoundComponent } from './components/404/404.component';
import { StatusRoute } from './status.routes';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    imports: [
        CommonModule,
        MaterialModule,
        SharedModule,
        StatusRoute
    ],
    declarations: [
        UnauthorizedComponent, NotFoundComponent
    ]
})

export class StatusModule { }
