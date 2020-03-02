import { Component, OnInit } from '@angular/core';
import { Role } from '../models/role.model';
import { Store, select } from '@ngrx/store';
import * as fromRole from '../reducers';
import { StartGet } from '../actions/role';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { RolesService } from '../services/role.service';
import { MatSnackBar } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'view-role',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewRoleComponent implements OnInit {
    role: Role = new Role();
    addRoleForm: FormGroup;

    constructor(private store: Store<fromRole.RoleState>, private route: ActivatedRoute, private formBuilder: FormBuilder, private roleService: RolesService, private snackBar: MatSnackBar, private translateService: TranslateService) {
        this.addRoleForm = this.formBuilder.group({
            role: ''
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromRole.selectGetResult)).subscribe((role: Role) => {
            if (!role) {
                return;
            }

            this.role = role;
        });
        this.refresh();
    }

    onSubmit(form: any) {
        this.role.Users.push(form.role);
        this.update();
    }

    remove(user : string) {
        var users = this.role.Users;
        var index = users.indexOf(user);
        users.splice(index, 1);
        this.update();
    }

    private refresh() {
        var id = this.route.snapshot.params['id'];
        var startGet = new StartGet(id);
        this.store.dispatch(startGet);
    }

    private update() {
        var cancel = this.translateService.instant('CANCEL');
        var role = this.route.snapshot.params['id'];
        this.roleService.update(role, this.role.Users).subscribe(() => {
            this.snackBar.open(this.translateService.instant('ROLE_SAVED'), cancel, {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('ERROR_SAVE_ROLE'), cancel, {
                duration: 2000
            });
        });
    }
}
