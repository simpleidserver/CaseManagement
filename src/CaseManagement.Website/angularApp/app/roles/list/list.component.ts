import { Component, OnInit, ViewChild } from '@angular/core';
import * as fromRole from '../reducers';
import { Store, select } from '@ngrx/store';
import { Role } from '../models/role.model';
import { SearchRolesResult } from '../models/search-roles.model';
import { MatSort, MatPaginator } from '@angular/material';
import { StartSearch } from '../actions/role';
import { merge } from 'rxjs';

@Component({
    selector: 'list-roles',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListRolesComponent implements OnInit {
    displayedColumns: string[] = ['name', 'create_datetime', 'update_datetime', 'actions'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    roles$: Role[] = [];
    length: number;
    constructor(private store: Store<fromRole.RoleState>) { }

    ngOnInit() {
        this.store.pipe(select(fromRole.selectSearchResults)).subscribe((l: SearchRolesResult) => {
            if (!l || !l.Content) {
                return;
            }

            this.roles$ = l.Content;
            this.length = l.TotalLength;
        });
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

        let request = new StartSearch(startIndex, count, active, direction);
        this.store.dispatch(request);
    }
}
