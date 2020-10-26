import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import * as fromHumanTaskInstActions from '@app/stores/humantaskinstances/actions/humantaskinst.actions';
import { CreateHumanTaskInstance } from '@app/stores/humantaskinstances/parameters/create-humantaskinstance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { CreateHumanTaskInstanceDialog } from './create-humantaskinstance-dialog.component';

@Component({
    selector: 'view-humantaskdef-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDef implements OnInit {
    id: string;
    humanTaskDef: HumanTaskDef = null;
    isErrorOccured: boolean = false;
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW";

    constructor(
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private dialog: MatDialog,
        private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_GET_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.HUMANTASKDEF_DOESNT_EXIST'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.isErrorOccured = true;
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskInstActions.ActionTypes.COMPLETE_CREATE_HUMANTASKINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.HUMANTASKINSTANCE_CREATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskInstActions.ActionTypes.ERROR_CREATE_HUMANTASKINSTANCE))
            .subscribe((e: any) => {
                var msg = [];
                for (var k in e.error.error.errors) {
                    msg.push(k + " : " + e.error.error.errors[k].join(','));
                }

                this.snackBar.open(msg.join(';'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.id = e.id;
        });
        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }

    createHumanTaskInstance() {
        if (!this.humanTaskDef) {
            return;
        }

        const dialogRef = this.dialog.open(CreateHumanTaskInstanceDialog, {
            width: '800px',
            data: this.humanTaskDef
        });
        dialogRef.afterClosed().subscribe((cmd: CreateHumanTaskInstance) => {
            if (!cmd) {
                return
            }


            this.store.dispatch(new fromHumanTaskInstActions.CreateHumanTaskInstanceOperation(cmd));
        });

    }
}
