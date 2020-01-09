import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { CaseDefinition } from '../models/case-definition.model';
import { CaseFile } from '../models/case-file.model';
import * as fromCaseDefinitionsStates from './case-definitions-states';
import * as fromCaseFileStates from './case-file-states';
import { ActionTypes } from './view-case-file-actions';
let CmmnViewer = require('cmmn-js');

@Component({
    selector: 'view-case-file',
    templateUrl: './view-case-file.component.html',  
    styleUrls: ['./view-case-file.component.scss']
})
export class ViewCaseFileComponent implements OnInit, OnDestroy {
    displayedColumns: string[] = ['Name', 'Actions'];
    caseFile: CaseFile = {
        Id: null,
        Name: null,
        Description: null,
        Payload: null,
        CreateDateTime: null
    };
    caseDefinitions: CaseDefinition[] = [];
    length: number;
    isCaseFileLoading: boolean;
    isCaseFileErrorLoadOccured: boolean;
    isCaseDefinitionsLoading: boolean;
    isCaseDefinitionsErrorLoadOccured: boolean;
    caseFileStoreSubscription: any;
    caseFileDefinitionsSubscription: any;
    viewer: any;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(private caseFileStore: Store<fromCaseFileStates.CaseFileState>, private caseDefinitionsStore: Store<fromCaseDefinitionsStates.CaseDefinitionsState>, private route: ActivatedRoute) { }

    ngOnInit() {
        this.viewer = new CmmnViewer({
            container: '#canvas'
        });
        this.isCaseFileLoading = true;
        this.isCaseDefinitionsLoading = true;		
        this.isCaseFileErrorLoadOccured = false;
        this.isCaseDefinitionsErrorLoadOccured = false;
        this.caseFileStoreSubscription = this.caseFileStore.pipe(select('caseFile')).subscribe((st: fromCaseFileStates.CaseFileState) => {
            if (!st) {
                return;
            }

            this.isCaseFileLoading = st.isLoading;
            this.isCaseFileErrorLoadOccured = st.isErrorLoadOccured;
            if (st.caseFile) {
                this.caseFile = st.caseFile;
                this.viewer.importXML(st.caseFile.Payload, function (err: any) {
                    if (!err) {
                        this.viewer.get('canvas').zoom('fit-viewport');
                    }
                });
            }
        });
        this.caseFileDefinitionsSubscription = this.caseDefinitionsStore.pipe(select('caseDefinitions')).subscribe((st: fromCaseDefinitionsStates.CaseDefinitionsState) => {
            if (!st) {
                return;
            }

            this.isCaseDefinitionsLoading = st.isLoading;
            this.isCaseDefinitionsErrorLoadOccured = st.isErrorLoadOccured;
            if (st.caseDefinitions) {
                this.caseDefinitions = st.caseDefinitions.Content;
                this.length = st.caseDefinitions.TotalLength;
            }
        });

        this.init();
    }

    ngAfterViewInit() {
        merge(this.sort.sortChange, this.paginator.page).subscribe(() => this.refresh());
    }

    init() {
        let id = this.route.snapshot.params['id'];
        let request: any = {
            type: ActionTypes.LOADCASEFILE,
            caseFileId: id            
        };
        this.caseFileStore.dispatch(request);
        this.refresh();
    }

    refresh() {
        let id = this.route.snapshot.params['id'];
        let request: any = {
            type: ActionTypes.LOADCASEDEFINITIONS,
            order: this.sort.active,
            direction: this.sort.direction,
            count: this.paginator.pageSize,
            caseFileId: id
        };
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            request['startIndex'] = this.paginator.pageIndex * this.paginator.pageSize;
        } else {
            request['startIndex'] = 0;
        }

        if (this.paginator.pageSize) {
            request['count'] = this.paginator.pageSize;
        } else {
            request['count'] = 5;
        }

        this.isCaseDefinitionsLoading = true;
        this.caseDefinitionsStore.dispatch(request);
    }

    ngOnDestroy() {
        this.caseFileStoreSubscription.unsubscribe();
        this.caseFileDefinitionsSubscription.unsubscribe();
    }
}