import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { SearchTasksResult } from '@app/stores/tasks/models/search-tasks-result.model';
import { Task } from '@app/stores/tasks/models/task.model';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { SearchTasks } from '../../stores/tasks/actions/tasks.actions';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'list-tasks-component',
    templateUrl: './list.component.html',
    styleUrls: ['./list.component.scss']
})
export class ListTasksComponent implements OnInit {
    displayedColumns: string[] = ['priority', 'presentationName', 'presentationSubject', 'actualOwner', 'status', 'createdTime'];
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    baseTranslationKey: string = "TASKS.LIST";
    searchTasksForm: FormGroup;
    length: number;
    tasks$: Task[] = [];

    constructor(private store: Store<fromAppState.AppState>,
        private translate: TranslateService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private formBuilder: FormBuilder) {
        this.searchTasksForm = this.formBuilder.group({
            actualOwner: new FormControl(''),
            status: new FormControl('')
        });
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectTaskLstResult)).subscribe((l: SearchTasksResult) => {
            if (!l || !l.content) {
                return;
            }

            this.tasks$ = l.content;
            this.length = l.totalLength;
        });
        this.activatedRoute.queryParamMap.subscribe((p: any) => {
            this.sort.active = p.get('active');
            this.sort.direction = p.get('direction');
            this.paginator.pageSize = p.get('pageSize');
            this.paginator.pageIndex = p.get('pageIndex');
            const actualOwner = p.get('actualOwner');
            if (actualOwner) {
                this.searchTasksForm.get('actualOwner').setValue(actualOwner);
            }

            const status = p.get('status');
            if (status) {
                this.searchTasksForm.get('status').setValue(status.split(','));
            }

            this.refresh()
        });
        this.translate.onLangChange.subscribe(() =>
        {
            this.refresh();
        });
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => {
            this.refreshUrl();
        });
    }

    onSearchTasks() {
        this.refreshUrl();
    }

    refreshUrl() {
        const queryParams: any = {
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
            active: this.sort.active,
            direction: this.sort.direction
        };
        const actualOwner = this.searchTasksForm.get('actualOwner').value;
        const status = this.searchTasksForm.get('status').value;
        if (actualOwner) {
            queryParams['actualOwner'] = actualOwner;
        }

        if (status) {
            queryParams['status'] = status.join(',');
        }

        this.router.navigate(['.'], {
            relativeTo: this.activatedRoute,
            queryParams: queryParams
        });
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

        let active = this.getOrder();
        let direction = this.getDirection();
        let request = new SearchTasks(active, direction, count, startIndex, this.searchTasksForm.get('actualOwner').value, this.searchTasksForm.get('status').value);
        this.store.dispatch(request);
    }

    private getOrder() {
        let active = "createdTime";
        if (this.sort.active) {
            active = this.sort.active;
        }

        return active;
    }

    private getDirection() {
        let direction = "desc";
        if (this.sort.direction) {
            direction = this.sort.direction;
        }

        return direction;
    }
}