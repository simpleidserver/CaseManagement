import { Component, /* Inject, */OnDestroy, OnInit, /*ViewChild,*/ ViewEncapsulation } from '@angular/core';
// import { MatDialog, MatDialogRef, MatSort, MAT_DIALOG_DATA } from '@angular/material';
// import { ActivatedRoute } from '@angular/router';
// import { select, Store } from '@ngrx/store';
// import { CaseElementInstance, CaseInstance, StateHistory, TransitionHistory } from '../../casedefinitions/models/case-instance.model';
// import * as fromCaseFileActions from '../../casefiles/actions/case-files';
// import * as fromCaseInstanceActions from '../../casedefinitions/actions/case-instances';
// import * as fromCaseDefinitionActions from '../../casedefinitions/actions/case-definitions';
// import * as fromCaseInstances from '../reducers';
// import { CaseFileItem } from '../../casedefinitions/models/case-file-item.model';
// import { CaseFile } from '../../casefiles/models/case-file.model';
// import { CaseDefinition } from '../../casedefinitions/models/case-definition.model';
// let CmmnViewer = require('cmmn-js/lib/NavigatedViewer');

@Component({
    selector: 'view-case-instances',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCaseInstanceComponent implements OnInit, OnDestroy {
    /*
    caseInstanceContextLst$: any[] = new Array();
    caseFileItems$: CaseFileItem[] = new Array<CaseFileItem>();
    caseInstance$: CaseInstance = new CaseInstance();
    displayStateHistoriesColumns: string[] = ['state', 'datetime'];
    displayTransitionHistoriesColumns: string[] = ['transition', 'datetime'];
    displayCaseFileItemsColumns: string[] = ['value', 'datetime'];
    @ViewChild('caseInstanceStateHistoriesSort') caseInstanceStateHistoriesSort: MatSort;
    @ViewChild('caseInstanceTransitionHistoriesSort') caseInstanceTransitionHistoriesSort: MatSort;
    @ViewChild('caseFileItemsSort') caseFileItemsSort: MatSort;

    constructor(private caseInstanceStore: Store<fromCaseInstances.CaseInstancesState>, private route: ActivatedRoute, private dialog : MatDialog) { }

    */
    ngOnInit() {
        /*
        let self = this;
        let viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseFileItemsResult)).subscribe((st: CaseFileItem[]) => {
            this.caseFileItems$ = st;
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseFileResult)).subscribe((st: CaseFile) => {
            if (!st || !st.Payload) {
                return;
            }

            viewer.importXML(st.Payload, function () {
                let canvas = viewer.get('canvas');
                let overlays = viewer.get('overlays');
                let groupedElements = new Map();
                self.caseInstance$.Elements.forEach(function (elt: CaseElementInstance) {
                    if (!groupedElements.has(elt.DefinitionId)) {
                        groupedElements.set(elt.DefinitionId, [elt]);
                    } else {
                        groupedElements.get(elt.DefinitionId).push(elt);
                    }
                });
                groupedElements.forEach(function (values: any[], key: any) {
                    overlays.remove(key);
                    let id = overlays.add(key, "note", {
                        position: {
                            top: -5,
                            right: -5
                        },
                        html: '<div class="nb-instances" data-eltdefinitionid="' + key + '">' + values.length + '</div>'
                    });
                    let elt = overlays.get(id);
                    elt.htmlContainer.onclick = function (evt: any) {
                        let eltdefinitionid = evt.target.getAttribute('data-eltdefinitionid');
                        let elementInstances = groupedElements.get(eltdefinitionid);
                        if (elementInstances) {
                            self.dialog.open(CaseElementInstanceDialog, {
                                width: '800px',
                                data: elementInstances
                            });
                        }
                    };
                });

                canvas.zoom('fit-viewport');
            });
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseDefinitionResult)).subscribe((st: CaseDefinition) => {
            if (!st || !st.CaseFile) {
                return;
            }

            let loadCaseFileRequest = new fromCaseFileActions.StartGet(st.CaseFile);
            this.caseInstanceStore.dispatch(loadCaseFileRequest);
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseInstanceResult)).subscribe((st: CaseInstance) => {
            if (!st || !st.Id) {
                return;
            }

            this.caseInstance$ = st;
            this.caseInstanceContextLst$ = [];
            if (this.caseInstance$.Context) {
                for (let record in this.caseInstance$.Context) {
                    this.caseInstanceContextLst$.push({
                        name: record,
                        value: this.caseInstance$.Context[record]
                    });
                }
            }

            let loadCaseDefinitionRequest = new fromCaseDefinitionActions.StartGet(st.DefinitionId);
            this.caseInstanceStore.dispatch(loadCaseDefinitionRequest);
        });
        this.refresh();
        */
    }

    /*
    ngAfterViewInit() {
        this.caseInstanceStateHistoriesSort.sortChange.subscribe(() => this.sortCaseInstanceStateHistories());
        this.caseInstanceTransitionHistoriesSort.sortChange.subscribe(() => this.sortCaseInstanceTransitionHistories());
        this.caseFileItemsSort.sortChange.subscribe(() => this.sortCaseFileItems());
    }

    sortCaseInstanceStateHistories() {
        let active = this.caseInstanceStateHistoriesSort.active;
        let direction = this.caseInstanceStateHistoriesSort.direction;
        this.caseInstance$.StateHistories.sort(function (a: StateHistory, b: StateHistory) {
            if (active == "state") {
                if (direction == "asc") {
                    return a.State.localeCompare(b.State);
                }

                return b.State.localeCompare(a.State);
            }

            if (active == "datetime") {
                if (direction == "asc") {
                    return new Date(a.DateTime).getTime() - new Date(b.DateTime).getTime();
                }

                return new Date(b.DateTime).getTime() - new Date(a.DateTime).getTime();
            }
        });
        this.caseInstance$.StateHistories = [...this.caseInstance$.StateHistories];
    }

    sortCaseInstanceTransitionHistories() {
        let active = this.caseInstanceTransitionHistoriesSort.active;
        let direction = this.caseInstanceTransitionHistoriesSort.direction;
        this.caseInstance$.TransitionHistories.sort(function (a: TransitionHistory, b: TransitionHistory) {
            if (active == "transition") {
                if (direction == "asc") {
                    return a.Transition.localeCompare(b.Transition);
                }

                return b.Transition.localeCompare(a.Transition);
            }

            if (active == "datetime") {
                if (direction == "asc") {
                    return new Date(a.DateTime).getTime() - new Date(b.DateTime).getTime();
                }

                return new Date(b.DateTime).getTime() - new Date(a.DateTime).getTime();
            }
        });
        this.caseInstance$.TransitionHistories = [...this.caseInstance$.TransitionHistories];
    }

    sortCaseFileItems() {
        let active = this.caseFileItemsSort.active;
        let direction = this.caseFileItemsSort.direction;
        this.caseFileItems$.sort(function (a: CaseFileItem, b: CaseFileItem) {
            if (active == "value") {
                if (direction == "asc") {
                    return a.Value.localeCompare(b.Value);
                }

                return b.Value.localeCompare(a.Value);
            }

            if (active == "datetime") {
                if (direction == "asc") {
                    return new Date(a.CreateDateTime).getTime() - new Date(b.CreateDateTime).getTime();
                }

                return new Date(b.CreateDateTime).getTime() - new Date(a.CreateDateTime).getTime();
            }
        });
        this.caseFileItems$ = [...this.caseFileItems$];
    }

    refresh() {
        let loadCaseInstance = new fromCaseInstanceActions.StartGet(this.route.snapshot.params['id']);
        this.caseInstanceStore.dispatch(loadCaseInstance);
    }
    */
    ngOnDestroy() {
    }
}

/*
class ElementStateHistory {
    Id: string;
    State: string;
    DateTime: string;
}

class ElementTransitionHistory {
    Id: string;
    Transition: string;
    DateTime: string;
}
*/

@Component({
    selector: 'case-element-instance-dialog',
    templateUrl: 'case-element-instance-dialog.html',
})
export class CaseElementInstanceDialog {
    /*
    definitionId: string = null;
    stateHistories: ElementStateHistory[] = [];
    transitionHistories: ElementTransitionHistory[] = [];
    stateHistoriesColumns: string[] = ['id', 'state', 'datetime'];
    transitionHistoriesColumns: string[] = ['id', 'transition', 'datetime'];
    @ViewChild('stateHistoriesSort') stateHistoriesSort: MatSort;
    @ViewChild('transitionHistoriesSort') transitionHistoriesSort: MatSort;

    constructor(public dialogRef: MatDialogRef<CaseElementInstanceDialog>, @Inject(MAT_DIALOG_DATA) public data: CaseElementInstance[]) {
        if (data.length > 0) {
            this.definitionId = data[0].DefinitionId;
            let self = this;
            data.forEach(function (d: CaseElementInstance) {
                d.StateHistories.forEach(function (sh: StateHistory) {
                    let record = new ElementStateHistory();
                    record.DateTime = sh.DateTime;
                    record.State = sh.State;
                    record.Id = d.Id;
                    self.stateHistories.push(record);
                });

                d.TransitionHistories.forEach(function (th: TransitionHistory) {
                    let record = new ElementTransitionHistory();
                    record.DateTime = th.DateTime;
                    record.Transition = th.Transition;
                    record.Id = d.Id;
                    self.transitionHistories.push(record);
                });
            });
        }
    }

    ngAfterViewInit() {
        this.stateHistoriesSort.sortChange.subscribe(() => this.sortStateHistories());
        this.transitionHistoriesSort.sortChange.subscribe(() => this.sortTransitionHistories());
    }

    sortStateHistories() {
        let active = this.stateHistoriesSort.active;
        let direction = this.stateHistoriesSort.direction;
        this.stateHistories.sort(function (a: ElementStateHistory, b: ElementStateHistory) {
            if (active == "id") {
                if (direction == "asc") {
                    return a.Id.localeCompare(b.Id);
                }

                return b.Id.localeCompare(a.Id);
            }

            if (active == "state") {
                if (direction == "asc") {
                    return a.State.localeCompare(b.State);
                }

                return b.State.localeCompare(a.State);
            }

            if (active == "datetime") {
                if (direction == "asc") {
                    return new Date(a.DateTime).getTime() - new Date(b.DateTime).getTime();
                }

                return new Date(b.DateTime).getTime() - new Date(a.DateTime).getTime();
            }
        });
        this.stateHistories = [...this.stateHistories];
    }

    sortTransitionHistories() {
        let active = this.transitionHistoriesSort.active;
        let direction = this.transitionHistoriesSort.direction;
        this.transitionHistories.sort(function (a: ElementTransitionHistory, b: ElementTransitionHistory) {
            if (active == "id") {
                if (direction == "asc") {
                    return a.Id.localeCompare(b.Id);
                }

                return b.Id.localeCompare(a.Id);
            }

            if (active == "transition") {
                if (direction == "asc") {
                    return a.Transition.localeCompare(b.Transition);
                }

                return b.Transition.localeCompare(a.Transition);
            }

            if (active == "datetime") {
                if (direction == "asc") {
                    return new Date(a.DateTime).getTime() - new Date(b.DateTime).getTime();
                }

                return new Date(b.DateTime).getTime() - new Date(a.DateTime).getTime();
            }
        });
        this.transitionHistories = [...this.transitionHistories];
    }

    onNoClick() {
        this.dialogRef.close();
    }
    */
}