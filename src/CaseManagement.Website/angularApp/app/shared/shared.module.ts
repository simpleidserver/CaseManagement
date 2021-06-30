import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { TranslateEnumPipe } from '../pipes/translateenum.pipe';
import { TranslateObjPipe } from '../pipes/translateobj.pipe';

@NgModule({
    imports: [
    ],
    declarations: [
        TranslateEnumPipe,
        TranslateObjPipe
    ],
    exports: [
        CommonModule,
        RouterModule,
        TranslateModule,
        TranslateEnumPipe,
        TranslateObjPipe
    ]
})

export class SharedModule { }