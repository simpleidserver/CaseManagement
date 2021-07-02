import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { TranslateEnumPipe } from '../pipes/translateenum.pipe';

@NgModule({
    imports: [
    ],
    declarations: [
        TranslateEnumPipe
    ],
    exports: [
        CommonModule,
        RouterModule,
        TranslateModule,
        TranslateEnumPipe
    ]
})

export class SharedModule { }