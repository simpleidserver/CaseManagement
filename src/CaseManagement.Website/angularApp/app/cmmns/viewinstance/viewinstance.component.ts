import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import { CmmnPlanInstanceResult, CmmnPlanItemInstanceResult, TransitionHistoryResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import * as fromAppState from '@app/stores/appstate';
import { Store, select, ScannedActionsSubject } from '@ngrx/store';
import * as fromCmmnPlanInstanceActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import * as fromCmmnFileActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { MatSnackBar, MatDialog, MatTableDataSource, MatSort } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
import { ViewMessageDialog } from './view-message-dialog';
import { CmmnFileState } from '../../stores/cmmnfiles/reducers/cmmnfile.reducers';

let CmmnViewer = require('cmmn-js/lib/Viewer');

declare var $: any;

export class Instance {
    nbOccurrence: number;
    name: string;
    state: string;
    transition: string;
    message: string;
    executionDateTime: Date;
}

@Component({
    selector: 'view-cmmn-instance',
    templateUrl: './viewinstance.component.html',
    styleUrls: ['./viewinstance.component.scss']
})
export class ViewCmmnInstanceComponent implements OnInit, OnDestroy {
    cmmnFileListener: any;
    cmmnPlanInstanceListener: any;
    displayedColumns: string[] = ['name', 'executionDateTime', 'state', 'transition', 'nbOccurrence', 'message'];
    viewer: any;
    cmmnFileId: string;
    cmmnPlanInstanceId: string;
    cmmnFile: CmmnFile = new CmmnFile();
    cmmnPlanInstance: CmmnPlanInstanceResult = new CmmnPlanInstanceResult();
    instances$: MatTableDataSource<Instance> = new MatTableDataSource<Instance>();

    @ViewChild('instancesSort') instancesSort: MatSort;

    constructor(
        private activatedRoute: ActivatedRoute,
        private store: Store<fromAppState.AppState>,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private dialog: MatDialog,
    ) {

    }

    ngOnInit() {
        const self = this;
        this.instances$.data = [new Instance(), new Instance()];
        this.viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnPlanInstanceActions.ActionTypes.ERROR_GET_CMMN_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMNPLANINSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnFileActions.ActionTypes.ERROR_GET_CMMNFILE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMNFILE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.cmmnFileListener = this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((e: CmmnFileState) => {
            if (!e || !e.content) {
                return;
            }

            this.cmmnFile = e.content;
            this.viewer.importXML(e.content.payload, function () {
                self.displayExecution();
                const canvas = self.viewer.get('canvas');
                canvas.zoom('fit-viewport');
            });
        });
        this.cmmnPlanInstanceListener = this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe((e: CmmnPlanInstanceResult) => {
            if (!e) {
                return;
            }

            this.cmmnPlanInstance = e;
            const getCmmnFileRequest = new fromCmmnFileActions.GetCmmnFile(e.caseFileId);
            this.store.dispatch(getCmmnFileRequest);
        });
        this.cmmnFileId = this.activatedRoute.snapshot.params['id'];
        this.cmmnPlanInstanceId = this.activatedRoute.snapshot.params['instanceid'];
        this.refresh();
    }

    ngOnDestroy(): void {
        this.cmmnFileListener.unsubscribe();
        this.cmmnPlanInstanceListener.unsubscribe();
    }

    ngAfterViewInit() {
        this.instances$.sort = this.instancesSort;
    }

    viewMessage(json: string) {
        if (typeof json !== "string") {
            json = JSON.stringify(json);
        }

        this.dialog.open(ViewMessageDialog, {
            data: { json: json },
            width: '800px'
        });
    }

    displayExecution() {
        const self = this;
        const overlays = self.viewer.get('overlays');
        const elementRegistry = self.viewer.get('elementRegistry');
        let grouped: any = this.cmmnPlanInstance.children.filter((x: CmmnPlanItemInstanceResult) => {
            return x.state;
        }).reduce((rv: any, x: CmmnPlanItemInstanceResult) => {
            rv[x.eltId] = rv[x.eltId] || [];
            rv[x.eltId].push(x);
            return rv;
        }, {});
        for (var key in grouped) {
            let values: CmmnPlanItemInstanceResult[] = grouped[key];
            let ordered = values.sort((a: CmmnPlanItemInstanceResult, b: CmmnPlanItemInstanceResult) => {
                return b.nbOccurrence - a.nbOccurrence;
            });
            let firstValue = ordered[0];
            let eltReg = elementRegistry.get(firstValue.eltId);
            if (!eltReg) {
                continue;
            }

            let stateHtml = "<div class='state " + firstValue.state + "'>" + firstValue.state + "</div>";
            let nbOccurrenceHtml = "<div class='nbOccurrence'>" + values.length + "</div>";
            let overlayHtml: any = "<div data-id='" + firstValue.eltId + "' style='cursor: pointer !important; width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>"
            overlayHtml = $(overlayHtml);
            overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    right: 50
                },
                html: nbOccurrenceHtml
            });
            overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    right: 20
                },
                html: stateHtml
            });

            overlays.add(firstValue.eltId, {
                position: {
                    top: 0,
                    left: 0,
                },
                html: overlayHtml
            });
            $(overlayHtml).click(function () {
                const eltid = $(this).data('id');
                let elts = elementRegistry.getAll();
                elts.forEach(function (e: any) {
                    $("[data-element-id='" + e.id + "']").find(".djs-visual > rect").css("stroke", "black");
                    $("[data-element-id='" + e.id + "']").find(".djs-visual > polygon").css("stroke", "black");
                });
                $("[data-element-id='" + eltid + "']").find(".djs-visual > rect").css("stroke", "red");
                $("[data-element-id='" + eltid + "']").find(".djs-visual > polygon").css("stroke", "red");
                self.displayInstances(grouped[eltid]);
            });
        }
    }

    displayInstances(arr: CmmnPlanItemInstanceResult[]) {
        const self = this;
        let records : Instance[] = [];
        arr.forEach(function (r: CmmnPlanItemInstanceResult) {
            r.transitionHistories.forEach(function (th: TransitionHistoryResult) {
                let record = new Instance();
                record.executionDateTime = th.executionDateTime;
                record.transition = th.transition;
                record.name = r.name;
                record.nbOccurrence = r.nbOccurrence;
                record.state = r.state;
                record.message = th.message;
                records.push(record);
            });
        });

        self.instances$.data = records;
    }

    refresh() {
        const getCmmnPlanInstanceRequest = new fromCmmnPlanInstanceActions.GetCmmnPlanInstance(this.cmmnPlanInstanceId);
        this.store.dispatch(getCmmnPlanInstanceRequest);
    }
}