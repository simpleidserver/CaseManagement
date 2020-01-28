import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Performance } from '../models/performance.model';
import { ActionTypes } from './list-actions';
import * as fromListPerformanceState from './list-states';

@Component({
    selector: 'list-performance',
    templateUrl: './list.component.html',  
    styleUrls: ['./list.component.scss']
})

export class ListPerformanceComponent implements OnInit, OnDestroy {
    subscription: any;
    viewChart: any[] = [500, 300];
    threadsPerformance: any[] = [];
    memoryConsumedMBPerformance: any[] = [];
    interval: any;

    constructor(private performancesStore: Store<fromListPerformanceState.ListPerformancesState>, private datePipe: DatePipe) {
    }

    ngOnInit() {
        let self = this;
        this.subscription = this.performancesStore.pipe(select('performances')).subscribe((st: fromListPerformanceState.ListPerformancesState) => {
            if (!st) {
                return;
            }

            if (st.content) {
                self.threadsPerformance.forEach(function (r: any) {
                    r.series = [];
                });
                self.memoryConsumedMBPerformance.forEach(function (r: any) {
                    r.series = [];
                });
                st.content.Content.sort(function (a: Performance, b: Performance) {
                    return new Date(a.CaptureDateTime).getTime() - new Date(b.CaptureDateTime).getTime();
                });
                st.content.Content.forEach(function (performance: Performance) {
                    let threadDimension: any = null;
                    let memoryConsumedDimension: any = null;
                    self.threadsPerformance.forEach(function (th: any) {
                        if (th.name == performance.MachineName) {
                            threadDimension = th;
                            return;
                        }
                    });
                    self.memoryConsumedMBPerformance.forEach(function (mc: any) {
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

                self.threadsPerformance = [...self.threadsPerformance];
                self.memoryConsumedMBPerformance = [...self.memoryConsumedMBPerformance];
            }
        });
        this.refresh();
        this.interval = setInterval(() => {
            this.refresh();
        }, 4000);
    }

    refresh() {
        let request: any = {
            type: ActionTypes.PERFORMANCESLOAD,
            order: "datetime",
            direction: "desc",
            count: 30,
            startIndex: 0,
            startDateTime: this.getCurrentDate()
        };

        this.performancesStore.dispatch(request);
    }

    private getCurrentDate() {
        return this.getDate(new Date());
    }

    private getDate(d: Date) {
        return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
    }

    ngOnDestroy() {
        clearInterval(this.interval);
        this.subscription.unsubscribe();
    }
}