import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateFieldPipe } from '@app/infrastructure/pipes/translateFieldPipe';
import { MaterialModule } from '@app/shared/material.module';
import { SharedModule } from '@app/shared/shared.module';
import { ViewHumanTaskDefDeadlinesComponent } from './deadlines/deadlines.component';
import { ViewHumanTaskDefInfoComponent } from './info/info.component';
import { ViewHumanTaskDefRenderingComponent } from './rendering/rendering.component';
import { HumanTaskDefsViewRoutes } from './view.routes';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HumanTaskDefsViewRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [],
    declarations: [
        ViewHumanTaskDefRenderingComponent,
        ViewHumanTaskDefInfoComponent,
        ViewHumanTaskDefDeadlinesComponent,
        TranslateFieldPipe
    ],
    exports: [ ]
})

export class HumanTaskDefsViewModule { }
