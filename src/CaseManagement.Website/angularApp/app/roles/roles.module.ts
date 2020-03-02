import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MaterialModule } from '../shared/material.module';
import { SharedModule } from '../shared/shared.module';
import { RoleEffects } from './effects/role';
import { ListRolesComponent } from './list/list.component';
import * as reducers from './reducers';
import { RolesRoutes } from './roles.routes';
import { RolesService } from './services/role.service';
import { ViewRoleComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        MaterialModule,
        RolesRoutes,
        EffectsModule.forRoot([RoleEffects]),
        StoreModule.forRoot(reducers.appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [],
    declarations: [ListRolesComponent, ViewRoleComponent],
    providers: [RolesService]
})
export class RolesModule { }