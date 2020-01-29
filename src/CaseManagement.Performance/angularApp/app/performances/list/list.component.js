var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { ActionTypes } from './list-actions';
var ListPerformanceComponent = (function () {
    function ListPerformanceComponent(performancesStore, datePipe) {
        this.performancesStore = performancesStore;
        this.datePipe = datePipe;
        this.viewChart = [500, 300];
        this.threadsPerformance = [];
        this.memoryConsumedMBPerformance = [];
    }
    ListPerformanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.subscription = this.performancesStore.pipe(select('performances')).subscribe(function (st) {
            if (!st) {
                return;
            }
            if (st.content) {
                self.threadsPerformance.forEach(function (r) {
                    r.series = [];
                });
                self.memoryConsumedMBPerformance.forEach(function (r) {
                    r.series = [];
                });
                st.content.Content.sort(function (a, b) {
                    return new Date(a.CaptureDateTime).getTime() - new Date(b.CaptureDateTime).getTime();
                });
                st.content.Content.forEach(function (performance) {
                    var threadDimension = null;
                    var memoryConsumedDimension = null;
                    self.threadsPerformance.forEach(function (th) {
                        if (th.name == performance.MachineName) {
                            threadDimension = th;
                            return;
                        }
                    });
                    self.memoryConsumedMBPerformance.forEach(function (mc) {
                        if (mc.name == performance.MachineName) {
                            memoryConsumedDimension = mc;
                            return;
                        }
                    });
                    if (threadDimension == null) {
                        threadDimension = {
                            name: performance.MachineName,
                            series: []
                        };
                        self.threadsPerformance.push(threadDimension);
                    }
                    if (memoryConsumedDimension == null) {
                        memoryConsumedDimension = {
                            name: performance.MachineName,
                            series: []
                        };
                        self.memoryConsumedMBPerformance.push(memoryConsumedDimension);
                    }
                    threadDimension.series.push({ "name": self.datePipe.transform(performance.CaptureDateTime, 'medium'), "value": performance.NbWorkingThreads });
                    memoryConsumedDimension.series.push({ "name": self.datePipe.transform(performance.CaptureDateTime, 'medium'), "value": performance.MemoryConsumedMB });
                });
                self.threadsPerformance = __spreadArrays(self.threadsPerformance);
                self.memoryConsumedMBPerformance = __spreadArrays(self.memoryConsumedMBPerformance);
            }
        });
        this.refresh();
        this.interval = setInterval(function () {
            _this.refresh();
        }, 4000);
    };
    ListPerformanceComponent.prototype.refresh = function () {
        var request = {
            type: ActionTypes.PERFORMANCESLOAD,
            order: "datetime",
            direction: "desc",
            count: 30,
            startIndex: 0,
            startDateTime: this.getCurrentDate()
        };
        this.performancesStore.dispatch(request);
    };
    ListPerformanceComponent.prototype.getCurrentDate = function () {
        return this.getDate(new Date());
    };
    ListPerformanceComponent.prototype.getDate = function (d) {
        return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
    };
    ListPerformanceComponent.prototype.ngOnDestroy = function () {
        clearInterval(this.interval);
        this.subscription.unsubscribe();
    };
    ListPerformanceComponent = __decorate([
        Component({
            selector: 'list-performance',
            templateUrl: './list.component.html',
            styleUrls: ['./list.component.scss']
        }),
        __metadata("design:paramtypes", [Store, DatePipe])
    ], ListPerformanceComponent);
    return ListPerformanceComponent;
}());
export { ListPerformanceComponent };
//# sourceMappingURL=list.component.js.map