import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromBpmnInstanceActions from '@app/stores/bpmninstances/actions/bpmn-instances.actions';
import { BpmnInstance } from '@app/stores/bpmninstances/models/bpmn-instance.model';
import { SearchBpmnInstancesResult } from '@app/stores/bpmninstances/models/search-bpmn-instances-result.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'list-bpmn-instances',
    templateUrl: './instances.component.html',
    styleUrls: ['./instances.component.scss']
})
export class ListBpmnInstancesComponent implements OnInit, OnDestroy {
    displayedColumns: string[] = ['status', 'create_datetime', 'update_datetime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    bpmnInstances$: BpmnInstance[] = [];
    interval: any;

    constructor(private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectBpmnInstancesResult)).subscribe((searchBpmnInstancesResult: SearchBpmnInstancesResult) => {
            if (!searchBpmnInstancesResult) {
                return;
            }

            this.bpmnInstances$ = searchBpmnInstancesResult.content;
            this.length = searchBpmnInstancesResult.totalLength;
        });
        this.interval = setInterval(() => {
            this.refresh();
        }, 2000);
        this.refresh();
    }

    onSubmit() {
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    refresh() {
        let id = this.route.parent.snapshot.params['id'];
        let startIndex: number = 0;
        let count: number = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        } 

        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }

        let active = "create_datetime";
        let direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }

        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        const request = new fromBpmnInstanceActions.SearchBpmnInstances(active, direction, count, startIndex, id);
        this.store.dispatch(request);
    }

    ngOnDestroy() {
        if (this.interval) {
            clearInterval(this.interval);
        }
    }

    start(evt: any, bpmnInstance: BpmnInstance) {
        evt.preventDefault();
        const request = new fromBpmnInstanceActions.StartBpmnInstance(bpmnInstance.id);
        this.store.dispatch(request);
    }
}
