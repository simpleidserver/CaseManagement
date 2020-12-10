import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import * as fromCmmnPlanActions from '@app/stores/cmmnplans/actions/cmmn-plans.actions';
import { ScannedActionsSubject, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-cmmnplan',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewCmmnPlanComponent implements OnInit {
    id: string;
    cmmnFiles$: CmmnFile[] = [];
    cmmnFile: CmmnFile = new CmmnFile();

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService) {

    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnPlanActions.ActionTypes.ERROR_GET_CMMN_PLAN))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMN_PLAN'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.refresh();
    }

    refresh() {
        this.id = this.route.snapshot.params['id'];
        const request = new fromCmmnPlanActions.GetCmmnPlan(this.id);
        this.store.dispatch(request);
    }
}
