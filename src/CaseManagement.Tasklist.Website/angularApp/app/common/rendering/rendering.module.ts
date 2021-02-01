import { NgModule } from '@angular/core';
import { DynamicComponent } from './dynamic.component';
import { TxtComponent } from './txt/txt.component';
import { ColumnComponent } from './column/column.component';
import { RowComponent } from './row/row.component';
import { HeaderComponent } from './header/header.component';
import { SelectComponent } from './select/select.component';
import { ContainerComponent } from './container/container.component';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
    imports: [
        CommonModule,
        MaterialModule,
        SharedModule
    ],
    entryComponents: [
        ContainerComponent,
        TxtComponent,
        SelectComponent,
    ],
    declarations: [
        ContainerComponent,
        HeaderComponent,
        SelectComponent,
        ColumnComponent,
        RowComponent,
        TxtComponent,
        DynamicComponent
    ],
    exports: [
        ContainerComponent,
        HeaderComponent,
        SelectComponent,
        ColumnComponent,
        RowComponent,
        TxtComponent,
        DynamicComponent
    ]
})

export class RenderingModule { }
