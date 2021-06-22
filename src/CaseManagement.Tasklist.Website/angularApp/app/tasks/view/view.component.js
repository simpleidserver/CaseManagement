var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component, ViewChild } from '@angular/core';
import { MatSnackBar, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { Task } from '@app/stores/tasks/models/task.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewTaskComponent = (function () {
    function ViewTaskComponent(store, route, translate, snackBar, actions$) {
        this.store = store;
        this.route = route;
        this.translate = translate;
        this.snackBar = snackBar;
        this.actions$ = actions$;
        this.displayedColumns = ["eventTime", "userPrincipal", "eventType", "startOwner", "endOwner"];
        this.task = new Task();
        this.option = {
            type: 'container',
            children: []
        };
    }
    ViewTaskComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.COMPLETE_SUBMIT_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('TASKS.MESSAGES.TASK_COMPLETED'), _this.translate.instant('undo'), {
                duration: 2000
            });
            _this.refresh();
        });
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.ERROR_SUBMIT_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('TASKS.MESSAGES.ERROR_SUBMIT_TASK'), _this.translate.instant('undo'), {
                duration: 2000
            });
        });
        this.store.pipe(select(fromAppState.selectTask)).subscribe(function (r) {
            if (!r) {
                return;
            }
            if (r.task) {
                _this.task = r.task;
            }
            if (r.rendering) {
                _this.option = r.rendering;
            }
            if (r.searchTaskHistory) {
                _this.histories$ = new MatTableDataSource(r.searchTaskHistory.content);
                _this.histories$.sort = _this.sort;
            }
        });
        this.refresh();
    };
    ViewTaskComponent.prototype.refresh = function () {
        var id = this.route.snapshot.params['id'];
        var req = new fromTaskActions.RenderingTask(id, "eventTime", "desc");
        this.store.dispatch(req);
    };
    ViewTaskComponent.prototype.onSubmit = function () {
        var result = {};
        this.buildJSON(result, this.option);
        var id = this.route.snapshot.params['id'];
        var req = new fromTaskActions.SubmitTask(id, result);
        this.store.dispatch(req);
    };
    ViewTaskComponent.prototype.buildJSON = function (result, opt) {
        var _this = this;
        switch (opt.type) {
            case 'txt':
            case 'select':
                if (opt.value) {
                    result[opt.name] = opt.value;
                }
                break;
            default:
                if (opt.children && opt.children.length > 0) {
                    opt.children.forEach(function (r) {
                        _this.buildJSON(result, r);
                    });
                }
        }
    };
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ViewTaskComponent.prototype, "sort", void 0);
    ViewTaskComponent = __decorate([
        Component({
            selector: 'view-task-component',
            templateUrl: './view.component.html',
            styleUrls: ['./view.component.scss', '../../common/rendering/rendering.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            TranslateService,
            MatSnackBar,
            ScannedActionsSubject])
    ], ViewTaskComponent);
    return ViewTaskComponent;
}());
export { ViewTaskComponent };
//# sourceMappingURL=view.component.js.map