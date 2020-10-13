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

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HumanTaskDefsRoutes,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [AddHumanTaskDefDialog],
    declarations: [
        ViewHumanTaskDef,
        ListHumanTaskDef,
        AddHumanTaskDefDialog

    ],
    exports: [ ]
})

export class HumanTaskDefsModule { }
