import { NgModule } from '@angular/core';
import { NavigationComponent } from './components/navigation/navigation.component';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/material.module';

import { FormsModule } from '@angular/forms'


@NgModule({
    imports: [
    	SharedModule,
        MaterialModule,
        FormsModule
    ],

    declarations: [
        NavigationComponent
    ],

    exports: [
        NavigationComponent
    ]
})

export class NavigationModule { }