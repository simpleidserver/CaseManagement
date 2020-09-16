import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCasePlanInstanceActions from '@app/stores/caseplaninstances/actions/caseplaninstance.actions';
import { CasePlanInstanceResult, CasePlanItemInstanceResult } from '@app/stores/caseplaninstances/models/caseplaninstance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-case-instances',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCasePlanInstanceComponent implements OnInit {
    casePlanInstance: CasePlanInstanceResult = new CasePlanInstanceResult();
    activeActivities: CasePlanItemInstanceResult[] = [];
    enableActivities: CasePlanItemInstanceResult[] = [];
    completeActivities: CasePlanItemInstanceResult[] = [];
    milestones: CasePlanItemInstanceResult[] = [];
    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) { }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.COMPLETE_ENABLE_CASE_PLANINSTANCE_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('ACTIVITY_IS_ENABLED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCasePlanInstanceActions.ActionTypes.ERROR_ENABLE_CASE_PLANINSTANCE_ELT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CANNOT_ENABLE_ACTIVITY'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.refresh();
            });
        this.store.pipe(select(fromAppState.selectCasePlanInstanceResult)).subscribe((casePlanInstance: CasePlanInstanceResult) => {
            if (!casePlanInstance) {
                return;
            }

            this.casePlanInstance = casePlanInstance;
            this.activeActivities = casePlanInstance.children.filter((cp) => {
                return this.isActivity(cp) && cp.state === "Active";
            });
            this.enableActivities = casePlanInstance.children.filter((cp) => {
                return this.isActivity(cp) && cp.state === "Enabled";
            });
            this.completeActivities = casePlanInstance.children.filter((cp) => {
                return this.isActivity(cp) && cp.state === "Completed";
            });
            this.milestones = casePlanInstance.children.filter((cp) => {
                return cp.type === "MILESTONE";
            });
        });
        this.refresh();
    }

    enable(casePlanElementInstance: CasePlanItemInstanceResult) {
        const suspendCasePlanInstance = new fromCasePlanInstanceActions.EnableCasePlanInstanceElt(this.casePlanInstance.id, casePlanElementInstance.id);
        this.store.dispatch(suspendCasePlanInstance);
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromCasePlanInstanceActions.GetCasePlanInstance(id);
        this.store.dispatch(request);
    }

    private isActivity(casePlanElementInstance: CasePlanItemInstanceResult) {
        return casePlanElementInstance.type === "TASK" ||
            casePlanElementInstance.type === "HUMANTASK" ||
            casePlanElementInstance.type === "PROCESSTASK" ||
            casePlanElementInstance.type === "EMPTYTASK";
    }
}