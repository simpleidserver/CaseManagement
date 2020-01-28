import { Component, OnDestroy, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { DailyStatistic } from '../models/dailystatistic.model';
import { ActionTypes } from './home-actions';
import * as fromHomeSates from './home-states';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-home-component',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
    statisticSubscription: any;
    weekSubscription: any;
    monthSubscription: any;
    deployedSubscription: any;
    nbCaseDefinitions: number = 0;
    nbCaseFiles: number = 0;
    viewPie: any[] = [300, 300];
    viewChart: any[] = [500, 300];
    caseStatistic: any[] = [
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
    caseStatisticColorScheme = {
        domain: ['#d3d3d3', '#008000', '#ffff00', '#FF0000', '#FFA500', '#808080']
    };
    caseWeekStatistic: any[] = [
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
    caseMonthStatistic: any[] = [
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
    formStatistic: any[] = [
        {
            "name": "Created",
            "value": 0
        },
        {
            "name": "Confirmed",
            "value": 0
        }
    ];
    formStatisticColorScheme = {
        domain: ['#808080', '#008000']
    };
    formWeekStatistic: any[] = [
        {
            "name": "Created",
            "series": []
        },
        {
            "name": "Confirmed",
            "series": []
        }
    ];
    formMonthStatistic: any[] = [
        {
            "name": "Created",
            "series": []
        },
        {
            "name": "Confirmed",
            "series": []
        }
    ];
    activationStatisticColorScheme = {
        domain: ['#808080', '#008000']
    };
    activationStatistic: any[] = [
        {
            "name": "Created",
            "value": 0
        },
        {
            "name": "Confirmed",
            "value": 0
        }
    ];
    activationWeekStatistic: any[] = [
        {
            "name": "Created",
            "series": []
        },
        {
            "name": "Confirmed",
            "series": []
        }
    ];
    activationMonthStatistic: any[] = [
        {
            "name": "Created",
            "series": []
        },
        {
            "name": "Confirmed",
            "series": []
        }
    ];
    constructor(private statisticStore: Store<fromHomeSates.StatisticState>, private weekStatisticStore: Store<fromHomeSates.WeekStatisticsState>, private monthStatisticStore: Store<fromHomeSates.MonthStatisticsState>, private deployedStore : Store<fromHomeSates.DeployedState>, private datePipe: DatePipe) { }

    ngOnInit() {
        let self = this;
        this.statisticSubscription = this.statisticStore.pipe(select('statistic')).subscribe((st: fromHomeSates.StatisticState) => {
            if (!st) {
                return;
            }

            if (st.content) {
                this.caseStatistic = [
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
                this.formStatistic = [
                    {
                        "name": "Created",
                        "value": st.content.NbCreatedForms
                    },
                    {
                        "name": "Confirmed",
                        "value": st.content.NbConfirmedForms
                    }
                ];
                this.activationStatistic = [
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
        this.weekSubscription = this.weekStatisticStore.pipe(select('weekStatistics')).subscribe((st: fromHomeSates.WeekStatisticsState) => {
            if (!st) {
                return;
            }

            if (st.content) {
                let caseWeekResult: any[] = [
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
                let formWeekResult: any[] = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                let activationWeekResult: any[] = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                st.content.Content.forEach(function (elt: DailyStatistic) {
                    caseWeekResult[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbActiveCases
                    });
                    caseWeekResult[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCompletedCases
                    });
                    caseWeekResult[2].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbTerminatedCases
                    });
                    caseWeekResult[3].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbFailedCases
                    });
                    caseWeekResult[4].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbSuspendedCases
                    });
                    caseWeekResult[5].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbClosedCases
                    });
                    formWeekResult[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    formWeekResult[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                    activationWeekResult[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    activationWeekResult[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                });

                this.caseWeekStatistic = caseWeekResult;
                this.formWeekStatistic = formWeekResult;
                this.activationWeekStatistic = activationWeekResult;
            }
        });
        this.monthSubscription = this.monthStatisticStore.pipe(select('monthStatistics')).subscribe((st: fromHomeSates.WeekStatisticsState) => {
            if (!st) {
                return;
            }

            if (st.content) {
                let caseMonthResult: any[] = [
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
                let formMonthResult: any[] = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                let activationMonthResult: any[] = [
                    {
                        "name": "Created",
                        "series": []
                    },
                    {
                        "name": "Confirmed",
                        "series": []
                    }
                ];
                st.content.Content.forEach(function (elt: DailyStatistic) {
                    caseMonthResult[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbActiveCases
                    });
                    caseMonthResult[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCompletedCases
                    });
                    caseMonthResult[2].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbTerminatedCases
                    });
                    caseMonthResult[3].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbFailedCases
                    });
                    caseMonthResult[4].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbSuspendedCases
                    });
                    caseMonthResult[5].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbClosedCases
                    });
                    formMonthResult[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    formMonthResult[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                    activationMonthResult[0].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbCreatedForms
                    });
                    activationMonthResult[1].series.push({
                        "name": self.datePipe.transform(elt.DateTime, 'mediumDate'),
                        "value": elt.NbConfirmedForms
                    });
                });

                this.caseMonthStatistic = caseMonthResult;
                this.formMonthStatistic = formMonthResult;
                this.activationMonthStatistic = activationMonthResult;
            }
        });        
        this.deployedSubscription = this.deployedStore.pipe(select('deployed')).subscribe((st: fromHomeSates.DeployedState) => {
            if (st.nbCaseDefinitions) {
                this.nbCaseDefinitions = st.nbCaseDefinitions.Count;
                this.nbCaseFiles = st.nbCaseFiles.Count;
            }
        });
        this.refresh();
    }

    refresh() {
        let loadStatisticRequest: any = {
            type: ActionTypes.STATISTICLOAD
        };
        let loadWeekStatisticsRequest: any = {
            type: ActionTypes.SEARCHWEEKSTATISTICS,
            count: 100
        };
        let loadMonthStatisticsRequest: any = {
            type: ActionTypes.SEARCHMONTHSTATISTICS,
            count: 100
        };
        let loadDeployedRequest: any = {
            type: ActionTypes.DEPLOYEDLOAD
        };
        this.statisticStore.dispatch(loadStatisticRequest);
        this.weekStatisticStore.dispatch(loadWeekStatisticsRequest);
        this.monthStatisticStore.dispatch(loadMonthStatisticsRequest);
        this.deployedStore.dispatch(loadDeployedRequest);
    }

    ngOnDestroy() {
        this.statisticSubscription.unsubscribe();
        this.weekSubscription.unsubscribe();
        this.monthSubscription.unsubscribe();
    }
}
