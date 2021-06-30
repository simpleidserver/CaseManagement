import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import * as fromDelegateConfigurationActions from '@app/stores/delegateconfigurations/actions/delegateconfiguration.actions';
import { DelegateConfiguration } from '@app/stores/delegateconfigurations/models/delegateconfiguration.model';
import { SearchDelegateConfigurationResult } from '@app/stores/delegateconfigurations/models/searchdelegateconfiguration.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';

@Component({
    selector: 'list-delegates',
    templateUrl: './listdelegates.component.html',
    styleUrls: ['./listdelegates.component.scss']
})
export class ListDelegateConfigurationComponent implements OnInit, OnDestroy {
    listener: any;
    displayedColumns: string[] = [ 'displayName', 'create_datetime', 'update_datetime' ];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    length: number;
    content$: DelegateConfiguration[] = [];

    constructor(private store: Store<fromAppState.AppState>) {
    }

    ngOnInit() {
        this.listener = this.store.pipe(select(fromAppState.selectDelegateConfigurationLstResult)).subscribe((searchDelegateConfiguration: SearchDelegateConfigurationResult) => {
            if (!searchDelegateConfiguration) {
                return;
            }

            this.content$ = searchDelegateConfiguration.content;
            this.length = searchDelegateConfiguration.totalLength;
        });
        this.refresh();
    }

    ngOnDestroy(): void {
        if (this.listener) {
            this.listener.unsubscribe();
        }
    }

    onSubmit() {
        this.refresh();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    refresh() {
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

        const request = new fromDelegateConfigurationActions.SearchDelegateConfiguration(active, direction, count, startIndex);
        this.store.dispatch(request);
    }
}
