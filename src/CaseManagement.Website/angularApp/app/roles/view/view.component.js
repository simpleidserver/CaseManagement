var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { Role } from '../models/role.model';
import { Store, select } from '@ngrx/store';
import * as fromRole from '../reducers';
import { StartGet } from '../actions/role';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { RolesService } from '../services/role.service';
import { MatSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
var ViewRoleComponent = (function () {
    function ViewRoleComponent(store, route, formBuilder, roleService, snackBar, translateService) {
        this.store = store;
        this.route = route;
        this.formBuilder = formBuilder;
        this.roleService = roleService;
        this.snackBar = snackBar;
        this.translateService = translateService;
        this.role = new Role();
        this.addRoleForm = this.formBuilder.group({
            role: ''
        });
    }
    ViewRoleComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromRole.selectGetResult)).subscribe(function (role) {
            if (!role) {
                return;
            }
            _this.role = role;
        });
        this.refresh();
    };
    ViewRoleComponent.prototype.onSubmit = function (form) {
        this.role.Users.push(form.role);
        this.update();
    };
    ViewRoleComponent.prototype.remove = function (user) {
        var users = this.role.Users;
        var index = users.indexOf(user);
        users.splice(index, 1);
        this.update();
    };
    ViewRoleComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var startGet = new StartGet(id);
        this.store.dispatch(startGet);
    };
    ViewRoleComponent.prototype.update = function () {
        var _this = this;
        var cancel = this.translateService.instant('CANCEL');
        var role = this.route.snapshot.params['id'];
        this.roleService.update(role, this.role.Users).subscribe(function () {
            _this.snackBar.open(_this.translateService.instant('ROLE_SAVED'), cancel, {
                duration: 2000
            });
        }, function () {
            _this.snackBar.open(_this.translateService.instant('ERROR_SAVE_ROLE'), cancel, {
                duration: 2000
            });
        });
    };
    ViewRoleComponent = __decorate([
        Component({
            selector: 'view-role',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss']
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute, FormBuilder, RolesService, MatSnackBar, TranslateService])
    ], ViewRoleComponent);
    return ViewRoleComponent;
}());
export { ViewRoleComponent };
//# sourceMappingURL=view.component.js.map