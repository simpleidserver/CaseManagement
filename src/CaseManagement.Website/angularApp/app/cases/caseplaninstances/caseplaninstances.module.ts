import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
import { CasePlanInstancesRoutes } from './caseplaninstances.routes';
import { CasePlanInstanceEffects } from './effects/caseplaninstance';
import { ListCasePlanInstancesComponent } from './list/list.component';
import * as reducers from './reducers';
import { CasePlanInstanceService } from './services/caseplaninstance.service';

@NgModule({
    imports: [
        CommonModule,
        CasePlanInstancesRoutes,
        SharedModule,
        MaterialModule,
        EffectsModule.forRoot([CasePlanInstanceEffects]),
        StoreModule.forRoot(reducers.appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    declarations: [ListCasePlanInstancesComponent ],
    providers: [ CasePlanInstanceService ]
})

export class CasePlanInstancesModule { }