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
import { select, Store } from '@ngrx/store';
import { ActionTypes } from './statistics-actions';
import { DatePipe } from '@angular/common';
var StatisticsComponent = (function () {
    function StatisticsComponent(statisticStore, weekStatisticStore, monthStatisticStore, deployedStore, datePipe) {
        this.statisticStore = statisticStore;
        this.weekStatisticStore = weekStatisticStore;
        this.monthStatisticStore = monthStatisticStore;
        this.deployedStore = deployedStore;
        this.datePipe = datePipe;
        this.nbCasePlans = 0;
        this.nbCaseFiles = 0;
        this.viewPie = [300, 300];
        this.viewChart = [500, 300];
        this.caseStatistic = [
            {
                "name": "Active",
                "value": 0
            },
            {
                "name": "Completed",
                "value": 0
            },
            {
                "name": "Terminated",
                "value": 0
            },
            {
                "name": "Failed",
                "value": 0
            },
            {
                "name": "Suspended",
                "value": 0
            },
            {
                "name": "Closed",
                "value": 0
            }
        ];
        this.caseStatisticColorScheme = {
            domain: ['#d3d3d3', '#008000', '#ffff00', '#FF0000', '#FFA500', '#808080']
        };
        this.caseWeekStatistic = [
            {
                "name": "Active",
                "series": []
            },
            {
                "name": "Completed",
                "series": []
            },
            {
                "name": "Terminated",
                "series": []
            },
            {
                "name": "Failed",
                "series": []
            },
            {
                "name": "Suspended",
                "series": []
            },
            {
                "name": "Closed",
                "series": []
            }
        ];
        this.caseMonthStatistic = [
            {
                "name": "Active",
                "series": []
            },
            {
                "name": "Completed",
                "series": []
            },
            {
                "name": "Terminated",
                "series": []
            },
            {
                "name": "Failed",
                "series": []
            },
            {
                "name": "Suspended",
                "series": []
            },
            {
                "name": "Closed",
                "series": []
            }
        ];
        this.formStatistic = [
            {
                "name": "Created",
                "value": 0
            },
            {
                "name": "Confirmed",
                "value": 0
            }
        ];
        this.formStatisticColorScheme = {
            domain: ['#808080', '#008000']
        };
        this.formWeekStatistic = [
            {
                "name": "Created",
                "series": []
            },
            {
                "name": "Confirmed",
                "series": []
            }
        ];
        this.formMonthStatistic = [
            {
                "name": "Created",
                "series": []
            },
            {
                "name": "Confirmed",
                "series": []
            }
        ];
    }
    StatisticsComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        this.statisticSubscription = this.statisticStore.pipe(select('statistic')).subscribe(function (st) {
            if (!st) {
                return;
            }
            if (st.content) {
                _this.caseStatistic = [
                    {
                        "name": "Active",
                        "value": st.content.NbActiveCases
                    },
                    {
                        "name": "Completed",
                        "value": st.content.NbCompletedCases
                    },
                    {
                        "name": "Terminated",
                        "value": st.content.NbTerminatedCases
                    },
                    {
                        "name": "Failed",
                        "value": st.content.NbFailedCases
                    },
                    {
                        "name": "Suspended",
                        "value": st.content.NbSuspendedCases
                    },
                    {
                        "name": "Closed",
                        "value": st.content.NbClosedCases
                    }
                ];
                _this.formStatistic = [
                    {
                        "name": "Created",
                        "value": st.content.NbCreatedForms
                    },
                    {
                        "name": "Confirmed",
                        "value": st.content.NbConfirmedForms
                    }
                ];
            }
        });
        this.weekSubscription = this.weekStatisticStore.pipe(select('weekStatistics')).subscribe(function (st) {
            if (!st) {
                return;
            }
            if (st.content) {
                var caseWeekResult_1 = [
                    {
                        "name": "Active",
                        "series": []
                    },
                    {
                        "name": "Completed",
                        "series": []
                    },
                    {
                        "name": "Terminated",
                        "series": []
                    },
                    {
                        "name": "Failed",
                        "series": []
                    },
                    {
                        "name": "Suspended",
                        "series": []
                    },
                    {
                        "name": "Closed",
                        "series": []
                    }
                ];
                var formWeekResult_1 = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                var activationWeekResult_1 = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                st.content.Content.forEach(function (elt) {
                    caseWeekResult_1[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbActiveCases
                    });
                    caseWeekResult_1[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCompletedCases
                    });
                    caseWeekResult_1[2].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbTerminatedCases
                    });
                    caseWeekResult_1[3].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbFailedCases
                    });
                    caseWeekResult_1[4].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbSuspendedCases
                    });
                    caseWeekResult_1[5].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbClosedCases
                    });
                    formWeekResult_1[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    formWeekResult_1[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                    activationWeekResult_1[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    activationWeekResult_1[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                });
                _this.caseWeekStatistic = caseWeekResult_1;
                _this.formWeekStatistic = formWeekResult_1;
            }
        });
        this.monthSubscription = this.monthStatisticStore.pipe(select('monthStatistics')).subscribe(function (st) {
            if (!st) {
                return;
            }
            if (st.content) {
                var caseMonthResult_1 = [
                    {
                        "name": "Active",
                        "series": []
                    },
                    {
                        "name": "Completed",
                        "series": []
                    },
                    {
                        "name": "Terminated",
                        "series": []
                    },
                    {
                        "name": "Failed",
                        "series": []
                    },
                    {
                        "name": "Suspended",
                        "series": []
                    },
                    {
                        "name": "Closed",
                        "series": []
                    }
                ];
                var formMonthResult_1 = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                st.content.Content.forEach(function (elt) {
                    caseMonthResult_1[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbActiveCases
                    });
                    caseMonthResult_1[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCompletedCases
                    });
                    caseMonthResult_1[2].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbTerminatedCases
                    });
                    caseMonthResult_1[3].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbFailedCases
                    });
                    caseMonthResult_1[4].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbSuspendedCases
                    });
                    caseMonthResult_1[5].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbClosedCases
                    });
                    formMonthResult_1[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    formMonthResult_1[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                });
                _this.caseMonthStatistic = caseMonthResult_1;
                _this.formMonthStatistic = formMonthResult_1;
            }
        });
        this.deployedSubscription = this.deployedStore.pipe(select('deployed')).subscribe(function (st) {
            if (st.content) {
                _this.nbCasePlans = st.content.NbCasePlans;
                _this.nbCaseFiles = st.content.NbCaseFiles;
            }
        });
        this.refresh();
    };
    StatisticsComponent.prototype.refresh = function () {
        var loadStatisticRequest = {
            type: ActionTypes.STATISTICLOAD
        };
        var loadWeekStatisticsRequest = {
            type: ActionTypes.SEARCHWEEKSTATISTICS,
            count: 100,
            startIndex: 0
        };
        var loadMonthStatisticsRequest = {
            type: ActionTypes.SEARCHMONTHSTATISTICS,
            count: 100,
            startIndex: 0
        };
        var loadDeployedRequest = {
            type: ActionTypes.DEPLOYEDLOAD
        };
        this.statisticStore.dispatch(loadStatisticRequest);
        this.weekStatisticStore.dispatch(loadWeekStatisticsRequest);
        this.monthStatisticStore.dispatch(loadMonthStatisticsRequest);
        this.deployedStore.dispatch(loadDeployedRequest);
    };
    StatisticsComponent.prototype.ngOnDestroy = function () {
        this.statisticSubscription.unsubscribe();
        this.weekSubscription.unsubscribe();
        this.monthSubscription.unsubscribe();
    };
    StatisticsComponent = __decorate([
        Component({
            selector: 'statistics-home-component',
            templateUrl: './statistics.component.html',
            styleUrls: ['./statistics.component.scss']
        }),
        __metadata("design:paramtypes", [Store, Store, Store, Store, DatePipe])
    ], StatisticsComponent);
    return StatisticsComponent;
}());
export { StatisticsComponent };
//# sourceMappingURL=statistics.component.js.map