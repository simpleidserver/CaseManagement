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
import { ActionTypes } from './home-actions';
var HomeComponent = (function () {
    function HomeComponent(statisticStore, weekStatisticStore, monthStatisticStore) {
        this.statisticStore = statisticStore;
        this.weekStatisticStore = weekStatisticStore;
        this.monthStatisticStore = monthStatisticStore;
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
        this.activationStatisticColorScheme = {
            domain: ['#808080', '#008000']
        };
        this.activationStatistic = [
            {
                "name": "Created",
                "value": 0
            },
            {
                "name": "Confirmed",
                "value": 0
            }
        ];
        this.activationWeekStatistic = [
            {
                "name": "Created",
                "series": []
            },
            {
                "name": "Confirmed",
                "series": []
            }
        ];
        this.activationMonthStatistic = [
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
    HomeComponent.prototype.ngOnInit = function () {
        var _this = this;
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
                _this.activationStatistic = [
                    {
                        "name": "Created",
                        "value": st.content.NbCreatedActivation
                    },
                    {
                        "name": "Confirmed",
                        "value": st.content.NbConfirmedActivation
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
                        "name": elt.DateTime,
                        "value": elt.NbActiveCases
                    });
                    caseWeekResult_1[1].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbCompletedCases
                    });
                    caseWeekResult_1[2].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbTerminatedCases
                    });
                    caseWeekResult_1[3].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbFailedCases
                    });
                    caseWeekResult_1[4].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbSuspendedCases
                    });
                    caseWeekResult_1[5].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbClosedCases
                    });
                    formWeekResult_1[0].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbCreatedForms
                    });
                    formWeekResult_1[1].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbConfirmedForms
                    });
                    activationWeekResult_1[0].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbCreatedForms
                    });
                    activationWeekResult_1[1].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbConfirmedForms
                    });
                });
                _this.caseWeekStatistic = caseWeekResult_1;
                _this.formWeekStatistic = formWeekResult_1;
                _this.activationWeekStatistic = activationWeekResult_1;
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
                var activationMonthResult_1 = [
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
                        "name": elt.DateTime,
                        "value": elt.NbActiveCases
                    });
                    caseMonthResult_1[1].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbCompletedCases
                    });
                    caseMonthResult_1[2].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbTerminatedCases
                    });
                    caseMonthResult_1[3].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbFailedCases
                    });
                    caseMonthResult_1[4].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbSuspendedCases
                    });
                    caseMonthResult_1[5].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbClosedCases
                    });
                    formMonthResult_1[0].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbCreatedForms
                    });
                    formMonthResult_1[1].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbConfirmedForms
                    });
                    activationMonthResult_1[0].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbCreatedForms
                    });
                    activationMonthResult_1[1].series.push({
                        "name": elt.DateTime,
                        "value": elt.NbConfirmedForms
                    });
                });
                _this.caseMonthStatistic = caseMonthResult_1;
                _this.formMonthStatistic = formMonthResult_1;
                _this.activationMonthStatistic = activationMonthResult_1;
            }
        });
        this.refresh();
    };
    HomeComponent.prototype.refresh = function () {
        var loadStatisticRequest = {
            type: ActionTypes.STATISTICLOAD
        };
        var loadWeekStatisticsRequest = {
            type: ActionTypes.SEARCHWEEKSTATISTICS,
            count: 100
        };
        var loadMonthStatisticsRequest = {
            type: ActionTypes.SEARCHMONTHSTATISTICS,
            count: 100
        };
        this.statisticStore.dispatch(loadStatisticRequest);
        this.weekStatisticStore.dispatch(loadWeekStatisticsRequest);
        this.monthStatisticStore.dispatch(loadMonthStatisticsRequest);
    };
    HomeComponent.prototype.ngOnDestroy = function () {
        this.statisticSubscription.unsubscribe();
        this.weekSubscription.unsubscribe();
        this.monthSubscription.unsubscribe();
    };
    HomeComponent = __decorate([
        Component({
            selector: 'app-home-component',
            templateUrl: './home.component.html',
            styleUrls: ['./home.component.scss']
        }),
        __metadata("design:paramtypes", [Store, Store, Store])
    ], HomeComponent);
    return HomeComponent;
}());
export { HomeComponent };
//# sourceMappingURL=home.component.js.map