var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
import { Component, Inject, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialog, MatDialogRef, MatSort, MAT_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { CaseInstance } from '../../casedefinitions/models/case-instance.model';
import * as fromCaseFileActions from '../../casefiles/actions/case-files';
import * as fromCaseInstanceActions from '../../casedefinitions/actions/case-instances';
import * as fromCaseDefinitionActions from '../../casedefinitions/actions/case-definitions';
import * as fromCaseInstances from '../reducers';
var CmmnViewer = require('cmmn-js/lib/NavigatedViewer');
var ViewCaseInstanceComponent = (function () {
    function ViewCaseInstanceComponent(caseInstanceStore, route, dialog) {
        this.caseInstanceStore = caseInstanceStore;
        this.route = route;
        this.dialog = dialog;
        this.caseInstanceContextLst$ = new Array();
        this.caseFileItems$ = new Array();
        this.caseInstance$ = new CaseInstance();
        this.displayStateHistoriesColumns = ['state', 'datetime'];
        this.displayTransitionHistoriesColumns = ['transition', 'datetime'];
        this.displayCaseFileItemsColumns = ['value', 'datetime'];
    }
    ViewCaseInstanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        var viewer = new CmmnViewer({
            container: "#canvas",
            keyboard: {
                bindTo: window
            }
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseFileItemsResult)).subscribe(function (st) {
            _this.caseFileItems$ = st;
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseFileResult)).subscribe(function (st) {
            if (!st || !st.Payload) {
                return;
            }
            viewer.importXML(st.Payload, function () {
                var canvas = viewer.get('canvas');
                var overlays = viewer.get('overlays');
                var groupedElements = new Map();
                self.caseInstance$.Elements.forEach(function (elt) {
                    if (!groupedElements.has(elt.DefinitionId)) {
                        groupedElements.set(elt.DefinitionId, [elt]);
                    }
                    else {
                        groupedElements.get(elt.DefinitionId).push(elt);
                    }
                });
                groupedElements.forEach(function (values, key) {
                    overlays.remove(key);
                    var id = overlays.add(key, "note", {
                        position: {
                            top: -5,
                            right: -5
                        },
                        html: '<div class="nb-instances" data-eltdefinitionid="' + key + '">' + values.length + '</div>'
                    });
                    var elt = overlays.get(id);
                    elt.htmlContainer.onclick = function (evt) {
                        var eltdefinitionid = evt.target.getAttribute('data-eltdefinitionid');
                        var elementInstances = groupedElements.get(eltdefinitionid);
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
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseDefinitionResult)).subscribe(function (st) {
            if (!st || !st.CaseFile) {
                return;
            }
            var loadCaseFileRequest = new fromCaseFileActions.StartGet(st.CaseFile);
            _this.caseInstanceStore.dispatch(loadCaseFileRequest);
        });
        this.caseInstanceStore.pipe(select(fromCaseInstances.selectCaseInstanceResult)).subscribe(function (st) {
            if (!st || !st.Id) {
                return;
            }
            _this.caseInstance$ = st;
            _this.caseInstanceContextLst$ = [];
            if (_this.caseInstance$.Context) {
                for (var record in _this.caseInstance$.Context) {
                    _this.caseInstanceContextLst$.push({
                        name: record,
                        value: _this.caseInstance$.Context[record]
                    });
                }
            }
            var loadCaseDefinitionRequest = new fromCaseDefinitionActions.StartGet(st.DefinitionId);
            _this.caseInstanceStore.dispatch(loadCaseDefinitionRequest);
        });
        this.refresh();
    };
    ViewCaseInstanceComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.caseInstanceStateHistoriesSort.sortChange.subscribe(function () { return _this.sortCaseInstanceStateHistories(); });
        this.caseInstanceTransitionHistoriesSort.sortChange.subscribe(function () { return _this.sortCaseInstanceTransitionHistories(); });
        this.caseFileItemsSort.sortChange.subscribe(function () { return _this.sortCaseFileItems(); });
    };
    ViewCaseInstanceComponent.prototype.sortCaseInstanceStateHistories = function () {
        var active = this.caseInstanceStateHistoriesSort.active;
        var direction = this.caseInstanceStateHistoriesSort.direction;
        this.caseInstance$.StateHistories.sort(function (a, b) {
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
        this.caseInstance$.StateHistories = __spreadArrays(this.caseInstance$.StateHistories);
    };
    ViewCaseInstanceComponent.prototype.sortCaseInstanceTransitionHistories = function () {
        var active = this.caseInstanceTransitionHistoriesSort.active;
        var direction = this.caseInstanceTransitionHistoriesSort.direction;
        this.caseInstance$.TransitionHistories.sort(function (a, b) {
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
        this.caseInstance$.TransitionHistories = __spreadArrays(this.caseInstance$.TransitionHistories);
    };
    ViewCaseInstanceComponent.prototype.sortCaseFileItems = function () {
        var active = this.caseFileItemsSort.active;
        var direction = this.caseFileItemsSort.direction;
        this.caseFileItems$.sort(function (a, b) {
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
        this.caseFileItems$ = __spreadArrays(this.caseFileItems$);
    };
    ViewCaseInstanceComponent.prototype.refresh = function () {
        var loadCaseInstance = new fromCaseInstanceActions.StartGet(this.route.snapshot.params['id']);
        this.caseInstanceStore.dispatch(loadCaseInstance);
    };
    ViewCaseInstanceComponent.prototype.ngOnDestroy = function () {
    };
    __decorate([
        ViewChild('caseInstanceStateHistoriesSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseInstanceComponent.prototype, "caseInstanceStateHistoriesSort", void 0);
    __decorate([
        ViewChild('caseInstanceTransitionHistoriesSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseInstanceComponent.prototype, "caseInstanceTransitionHistoriesSort", void 0);
    __decorate([
        ViewChild('caseFileItemsSort'),
        __metadata("design:type", MatSort)
    ], ViewCaseInstanceComponent.prototype, "caseFileItemsSort", void 0);
    ViewCaseInstanceComponent = __decorate([
        Component({
            selector: 'view-case-instances',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss'],
            encapsulation: ViewEncapsulation.None
        }),
        __metadata("design:paramtypes", [Store, ActivatedRoute, MatDialog])
    ], ViewCaseInstanceComponent);
    return ViewCaseInstanceComponent;
}());
export { ViewCaseInstanceComponent };
var ElementStateHistory = (function () {
    function ElementStateHistory() {
    }
    return ElementStateHistory;
}());
var ElementTransitionHistory = (function () {
    function ElementTransitionHistory() {
    }
    return ElementTransitionHistory;
}());
var CaseElementInstanceDialog = (function () {
    function CaseElementInstanceDialog(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
        this.definitionId = null;
        this.stateHistories = [];
        this.transitionHistories = [];
        this.stateHistoriesColumns = ['id', 'state', 'datetime'];
        this.transitionHistoriesColumns = ['id', 'transition', 'datetime'];
        if (data.length > 0) {
            this.definitionId = data[0].DefinitionId;
            var self_1 = this;
            data.forEach(function (d) {
                d.StateHistories.forEach(function (sh) {
                    var record = new ElementStateHistory();
                    record.DateTime = sh.DateTime;
                    record.State = sh.State;
                    record.Id = d.Id;
                    self_1.stateHistories.push(record);
                });
                d.TransitionHistories.forEach(function (th) {
                    var record = new ElementTransitionHistory();
                    record.DateTime = th.DateTime;
                    record.Transition = th.Transition;
                    record.Id = d.Id;
                    self_1.transitionHistories.push(record);
                });
            });
        }
    }
    CaseElementInstanceDialog.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.stateHistoriesSort.sortChange.subscribe(function () { return _this.sortStateHistories(); });
        this.transitionHistoriesSort.sortChange.subscribe(function () { return _this.sortTransitionHistories(); });
    };
    CaseElementInstanceDialog.prototype.sortStateHistories = function () {
        var active = this.stateHistoriesSort.active;
        var direction = this.stateHistoriesSort.direction;
        this.stateHistories.sort(function (a, b) {
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
        this.stateHistories = __spreadArrays(this.stateHistories);
    };
    CaseElementInstanceDialog.prototype.sortTransitionHistories = function () {
        var active = this.transitionHistoriesSort.active;
        var direction = this.transitionHistoriesSort.direction;
        this.transitionHistories.sort(function (a, b) {
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
        this.transitionHistories = __spreadArrays(this.transitionHistories);
    };
    CaseElementInstanceDialog.prototype.onNoClick = function () {
        this.dialogRef.close();
    };
    __decorate([
        ViewChild('stateHistoriesSort'),
        __metadata("design:type", MatSort)
    ], CaseElementInstanceDialog.prototype, "stateHistoriesSort", void 0);
    __decorate([
        ViewChild('transitionHistoriesSort'),
        __metadata("design:type", MatSort)
    ], CaseElementInstanceDialog.prototype, "transitionHistoriesSort", void 0);
    CaseElementInstanceDialog = __decorate([
        Component({
            selector: 'case-element-instance-dialog',
            templateUrl: 'case-element-instance-dialog.html',
        }),
        __param(1, Inject(MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [MatDialogRef, Array])
    ], CaseElementInstanceDialog);
    return CaseElementInstanceDialog;
}());
export { CaseElementInstanceDialog };
//# sourceMappingURL=view.component.js.map