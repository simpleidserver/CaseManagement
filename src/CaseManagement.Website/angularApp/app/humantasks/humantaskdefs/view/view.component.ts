import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
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
    id: string;
    isErrorOccured: boolean = false;
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW";

    constructor(
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
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
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.id = e.id;
        });
        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }
}
