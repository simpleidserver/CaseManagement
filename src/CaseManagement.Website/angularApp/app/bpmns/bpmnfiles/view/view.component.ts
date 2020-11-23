import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnFileActions from '@app/stores/bpmnfiles/actions/bpmn-files.actions';
import { ScannedActionsSubject, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-bpmn-file',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewBpmnFileComponent implements OnInit {
    id: string;

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private router: Router) {
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_GET_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_GET_BPMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.BPMNFILE_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PAYLOAD_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE_PAYLOAD))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE_PAYLOAD'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_PUBLISH_BPMNFILE))
            .subscribe((e) => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PUBLISHED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.router.navigate(["/bpmns/bpmnfiles/" + e.id]);
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_PUBLISH_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_PUBLISH_BPMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.COMPLETE_UPDATE_BPMNFILE_PAYLOAD))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.BPMNFILE_PAYLOAD_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromBpmnFileActions.ActionTypes.ERROR_UPDATE_BPMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('BPMN.MESSAGES.ERROR_UPDATE_BPMNFILE_PAYLOAD'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.id = this.route.snapshot.params['id'];
        this.refresh();
    }

    publish() {
        const act = new fromBpmnFileActions.PublishBpmnFile(this.id);
        this.store.dispatch(act);
    }

    refresh() {
        const request = new fromBpmnFileActions.GetBpmnFile(this.id);
        this.store.dispatch(request);
    }
}
