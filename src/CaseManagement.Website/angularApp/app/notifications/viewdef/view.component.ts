import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromNotificationDefActions from '@app/stores/notificationdefs/actions/notificationdef.actions';
import { NotificationDefinition } from '@app/stores/notificationdefs/models/notificationdef.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-notificationdef-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewNotificationDef implements OnInit {
    id: string;
    notificationDef: NotificationDefinition = new NotificationDefinition();
    isErrorOccured: boolean = false;

    constructor(
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.actions$.pipe(
            filter((action: any) => action.type === fromNotificationDefActions.ActionTypes.ERROR_GET_NOTIFICATIONDEF))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.UNKNOWN_NOTIFICATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
                this.isErrorOccured = true;
            });
        this.store.pipe(select(fromAppState.selectNotificationResult)).subscribe((e: NotificationDefinition) => {
            if (!e) {
                return;
            }

            this.notificationDef = e;
            this.id = e.id;
        });
        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromNotificationDefActions.GetNotificationOperation(id);
        this.store.dispatch(request);
    }
}
