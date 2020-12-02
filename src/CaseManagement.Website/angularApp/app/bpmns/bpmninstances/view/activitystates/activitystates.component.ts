import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { ActivityStateHistory, BpmnExecutionPath, BpmnExecutionPointer, BpmnInstance } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'list-activity-state',
    templateUrl: './activitystates.component.html',
    styleUrls: ['./activitystates.component.scss']
})
export class ActivityStatesComponent implements OnInit {
    displayedColumns: string[] = ['state', 'executionDateTime', 'message'];
    length: number;
    activityStates$: MatTableDataSource<ActivityStateHistory> = new MatTableDataSource<ActivityStateHistory>();
    @ViewChild(MatSort) sort: MatSort;
    bpmnInstance: BpmnInstance = null;

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectBpmnInstanceResult)).subscribe((e: BpmnInstance) => {
            if (!e) {
                return;
            }

            this.bpmnInstance = e;
        });

        this.route.parent.parent.params.subscribe(() => {
            this.refresh();
        });
        this.route.parent.params.subscribe(() => {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        this.activityStates$.sort = this.sort;
    }

    refresh() {
        if (!this.bpmnInstance) {
            return;
        }

        const pathid = this.route.parent.parent.snapshot.params['pathid'];
        const eltid = this.route.parent.snapshot.params['eltid'];
        const filteredExecutionPath = this.bpmnInstance.executionPaths.filter(function (ep: BpmnExecutionPath) {
            return ep.id === pathid;
        });
        if (filteredExecutionPath.length === 1) {
            const filteredElt = filteredExecutionPath[0].executionPointers.filter(function (ep: BpmnExecutionPointer) {
                return ep.id === eltid;
            });
            if (filteredElt.length === 1) {
                this.activityStates$.data = filteredElt[0].flowNodeInstance.activityStates;
            }
        }
    }
}
