var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromTaskActions from '@app/stores/tasks/actions/tasks.actions';
import { RenderingTask } from '@app/stores/tasks/actions/tasks.actions';
import { Task } from '@app/stores/tasks/models/task.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';
var ViewFormComponent = (function () {
    function ViewFormComponent(store, activatedRoute, actions$, snackBar, router, translate) {
        this.store = store;
        this.activatedRoute = activatedRoute;
        this.actions$ = actions$;
        this.snackBar = snackBar;
        this.router = router;
        this.translate = translate;
        this.option = {
            type: 'container',
            children: []
        };
        this.task = new Task();
    }
    ViewFormComponent.prototype.ngOnInit = function () {
        var _this = this;
        var id = this.activatedRoute.parent.snapshot.params['id'];
        this.actions$.pipe(filter(function (action) { return action.type === fromTaskActions.ActionTypes.COMPLETE_SUBMIT_TASK; }))
            .subscribe(function () {
            _this.snackBar.open(_this.translate.instant('TASKS.MESSAGES.TASK_COMPLETED'), _this.translate.instant('undo'), {
                duration: 2000
            });
            _this.router.navigate(['/cases/' + id]);
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
        });
        this.activatedRoute.params.subscribe(function () {
            _this.refresh();
        });
    };
    ViewFormComponent.prototype.refresh = function () {
        var id = this.activatedRoute.snapshot.params['formid'];
        var req = new RenderingTask(id, "eventTime", "desc");
        this.store.dispatch(req);
    };
    ViewFormComponent.prototype.onSubmit = function () {
        var result = {};
        this.buildJSON(result, this.option);
        var id = this.task.id;
        var req = new fromTaskActions.SubmitTask(id, result);
        this.store.dispatch(req);
    };
    ViewFormComponent.prototype.buildJSON = function (result, opt) {
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
    ViewFormComponent = __decorate([
        Component({
            selector: 'view-form-component',
            templateUrl: './viewform.component.html',
            styleUrls: ['./viewform.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            ActivatedRoute,
            ScannedActionsSubject,
            MatSnackBar,
            Router,
            TranslateService])
    ], ViewFormComponent);
    return ViewFormComponent;
}());
export { ViewFormComponent };
//# sourceMappingURL=viewform.component.js.map