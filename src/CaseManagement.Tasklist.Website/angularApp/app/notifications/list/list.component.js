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
import { MatPaginator, MatSort } from '@angular/material';
import * as fromAppState from '@app/stores/appstate';
import { select, Store } from '@ngrx/store';
import { merge } from 'rxjs';
import { SearchTasks } from '../../stores/tasks/actions/tasks.actions';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormControl } from '@angular/forms';
var ListTasksComponent = (function () {
    function ListTasksComponent(store, translate, formBuilder) {
        this.store = store;
        this.translate = translate;
        this.formBuilder = formBuilder;
        this.displayedColumns = ['priority', 'presentationName', 'presentationSubject', 'actualOwner', 'status', 'createdTime'];
        this.baseTranslationKey = "TASKS.LIST";
        this.tasks$ = [];
        this.searchTasksForm = this.formBuilder.group({
            actualOwner: new FormControl(''),
            status: new FormControl('')
        });
    }
    ListTasksComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.store.pipe(select(fromAppState.selectTaskLstResult)).subscribe(function (l) {
            if (!l || !l.content) {
                return;
            }
            _this.tasks$ = l.content;
            _this.length = l.totalLength;
        });
        this.translate.onLangChange.subscribe(function () {
            _this.refresh();
        });
        this.refresh();
    };
    ListTasksComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        merge(this.sort.sortChange, this.paginator.page).subscribe(function () { return _this.refresh(); });
    };
    ListTasksComponent.prototype.refresh = function () {
        var startIndex = 0;
        var count = 5;
        if (this.paginator.pageIndex && this.paginator.pageSize) {
            startIndex = this.paginator.pageIndex * this.paginator.pageSize;
        }
        if (this.paginator.pageSize) {
            count = this.paginator.pageSize;
        }
        var active = "create_datetime";
        var direction = "desc";
        if (this.sort.active) {
            active = this.sort.active;
        }
        if (this.sort.direction) {
            direction = this.sort.direction;
        }
        var request = new SearchTasks(active, direction, count, startIndex);
        this.store.dispatch(request);
    };
    __decorate([
        ViewChild(MatPaginator),
        __metadata("design:type", MatPaginator)
    ], ListTasksComponent.prototype, "paginator", void 0);
    __decorate([
        ViewChild(MatSort),
        __metadata("design:type", MatSort)
    ], ListTasksComponent.prototype, "sort", void 0);
    ListTasksComponent = __decorate([
        Component({
            selector: 'list-tasks-component',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store,
            TranslateService,
            FormBuilder])
    ], ListTasksComponent);
    return ListTasksComponent;
}());
export { ListTasksComponent };
//# sourceMappingURL=list.component.js.map