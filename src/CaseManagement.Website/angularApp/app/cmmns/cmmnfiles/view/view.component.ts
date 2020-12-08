import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { ScannedActionsSubject, Store, select } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { SearchCmmnFilesResult } from '@app/stores/cmmnfiles/models/search-cmmn-files-result.model';

@Component({
    selector: 'view-cmmnfile',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewCmmnFileComponent implements OnInit {
    id: string;
    cmmnFiles$: CmmnFile[] = [];
    cmmnFile: CmmnFile = new CmmnFile();

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private translateService: TranslateService,
        private actions$: ScannedActionsSubject,
        private router: Router) {
    }

    ngOnInit() {
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.CMMN_FILE_SAVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.COMPLETE_UPDATE_CMMNFILE_PAYLOAD))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.CMMN_FILE_PAYLOAD_SAVED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.ERROR_UPDATE_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_UPDATE_CMMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.COMPLETE_PUBLISH_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.COMPLETE_PUBLISH_CMMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.ERROR_PUBLISH_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_PUBLISH_CMMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectCmmnFileLstResult)).subscribe((e: SearchCmmnFilesResult) => {
            if (!e) {
                return;
            }

            this.cmmnFiles$ = e.content;
        });
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((e: CmmnFile) => {
            if (!e) {
                return;
            }

            this.cmmnFile = e;
            const request = new fromCmmnFileActions.SearchCmmnFiles("create_datetime", "desc", 10000, 0, null, e.fileId, false);
            this.store.dispatch(request);
        });
        this.route.params.subscribe(() => {
            this.refresh();
        });
        this.refresh();
    }

    publish() {
        const id = this.route.snapshot.params['id'];
        const act = new fromCmmnFileActions.PublishCmmnFile(id);
        this.store.dispatch(act);
    }

    refresh() {
        this.id = this.route.snapshot.params['id'];
        const request = new fromCmmnFileActions.GetCmmnFile(this.id);
        this.store.dispatch(request);
    }

    navigate(evt: any, cmmnFile: CmmnFile) {
        evt.preventDefault();
        this.router.navigate(['/cmmns/cmmnfiles/' + cmmnFile.id]);
    }
}
