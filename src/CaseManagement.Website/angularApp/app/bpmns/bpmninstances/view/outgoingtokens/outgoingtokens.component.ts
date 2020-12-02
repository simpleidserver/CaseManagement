import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { BpmnExecutionPath, BpmnExecutionPointer, BpmnInstance, BpmnMessageToken } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'list-outgoing-token',
    templateUrl: './outgoingtokens.component.html',
    styleUrls: ['./outgoingtokens.component.scss']
})
export class OutgoingTokensComponent implements OnInit {
    displayedColumns: string[] = ['name', 'content'];
    outgoingTokens$: BpmnMessageToken[] = [];
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
                this.outgoingTokens$ = filteredElt[0].outgoingTokens;
            }
        }
    }
}
