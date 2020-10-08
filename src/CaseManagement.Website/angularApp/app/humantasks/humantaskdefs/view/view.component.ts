import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { Store, ScannedActionsSubject } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'view-humantaskdef-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDef implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW";

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject) {
    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_HUMANASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.HUMANTASKDEF_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_HUMANASKDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.ERROR_UPDATE_HUMANTASKDEF'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });


        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }
}
