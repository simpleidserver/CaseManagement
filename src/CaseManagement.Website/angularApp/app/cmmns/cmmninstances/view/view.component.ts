import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute , Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromCmmnFilesActions from '@app/stores/cmmnfiles/actions/cmmn-files.actions';
import { CmmnFile } from '@app/stores/cmmnfiles/models/cmmn-file.model';
import * as fromCmmnInstancesActions from '@app/stores/cmmninstances/actions/cmmn-instances.actions';
import { CmmnPlanInstanceResult, CmmnPlanItemInstanceResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

let CmmnViewer = require('cmmn-js/lib/Viewer');

declare var $: any;

@Component({
    selector: 'view-cmmnplaninstance',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss']
})
export class ViewCmmnPlanInstanceComponent implements OnInit {
    viewer: any;
    cmmnFile: CmmnFile = null;
    cmmnPlanInstance: CmmnPlanInstanceResult = null;

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private snackBar: MatSnackBar,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private router: Router) { }

    ngOnInit() {
        this.viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.actions$.pipe(
            filter((action: any) => action.type === fromCmmnInstancesActions.ActionTypes.ERROR_GET_CMMN_PLANINSTANCE))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('CMMN.MESSAGES.ERROR_GET_CMMN_PLAN_INSTANCE'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.store.pipe(select(fromAppState.selectCmmnFileResult)).subscribe((e: CmmnFile) => {
            if (!e || !e.payload) {
                return;
            }

            this.cmmnFile = e;
            this.refresh();
        });
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe((e: CmmnPlanInstanceResult) => {
            if (!e) {
                return;
            }

            this.cmmnPlanInstance = e;
            const request = new fromCmmnFilesActions.GetCmmnFile(e.caseFileId);
            this.store.dispatch(request);
        });
        this.init();
    }

    refresh() {
        const self = this;
        if (!self.cmmnFile || !this.cmmnPlanInstance) {
            return;
        }

        this.viewer.importXML(self.cmmnFile.payload, function () {
            self.displayExecution();
            const canvas = self.viewer.get('canvas');
            canvas.zoom('fit-viewport');
        });
    }

    displayExecution() {
        const self = this;
        let id = this.route.snapshot.params['id'];
        const overlays = self.viewer.get('overlays');
        const elementRegistry = self.viewer.get('elementRegistry');
        let grouped: any = this.cmmnPlanInstance.children.reduce((rv: any, x: CmmnPlanItemInstanceResult) => {
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
            let overlayHtml : any = "<div data-id='" + firstValue.eltId + "' style='cursor: pointer !important; width:" + eltReg.width + "px;height:" + eltReg.height + "px;'></div>"
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
                self.router.navigate(['/cmmns/cmmninstances/' + id + '/' + eltid]);
            });
        }
    }

    init() {
        let id = this.route.snapshot.params['id'];
        const request = new fromCmmnInstancesActions.GetCmmnPlanInstance(id);
        this.store.dispatch(request);
    }
}
