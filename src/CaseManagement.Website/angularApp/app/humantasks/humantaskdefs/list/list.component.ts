import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatSnackBar } from '@angular/material';
import { AddHumanTaskDefDialog } from './add-humantaskdef-dialog.component';
import { ScannedActionsSubject, Store } from '@ngrx/store';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { filter } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'view-list-humantaskdef-component',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ListHumanTaskDef implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.LIST";

    constructor(
        private actions$: ScannedActionsSubject,
        private store: Store<fromAppState.AppState>,
        private dialog: MatDialog,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) {
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_ADD_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.HUMANTASK_ADDED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_ADD_HUMANTASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_ADD_HUMANTASK'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
    }

    addHumanTask() {
        const dialogRef = this.dialog.open(AddHumanTaskDefDialog, {
            width: '800px'
        });
        dialogRef.afterClosed().subscribe((e: any) => {
            if (!e) {
                return;
            }


            const request = new fromHumanTaskDefActions.AddHumanTaskDefOperation(e.name);
            this.store.dispatch(request);
        });
    }
}
