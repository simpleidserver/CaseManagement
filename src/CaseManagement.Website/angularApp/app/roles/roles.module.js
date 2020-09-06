var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
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
var RolesModule = (function () {
    function RolesModule() {
    }
    RolesModule = __decorate([
        NgModule({
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
    ], RolesModule);
    return RolesModule;
}());
export { RolesModule };
//# sourceMappingURL=roles.module.js.map