import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { StartGet } from '../actions/caseplaninstance';
import { CasePlanElementInstance, CasePlanInstance } from '../models/caseplaninstance.model';
import * as fromCasePlanInstance from '../reducers';
import { CasePlanInstanceService } from '../services/caseplaninstance.service';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'view-case-instances',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCasePlanInstanceComponent implements OnInit {
    casePlanInstance: CasePlanInstance = new CasePlanInstance();
    activeActivities: CasePlanElementInstance[] = [];
    enableActivities: CasePlanElementInstance[] = [];
    completeActivities: CasePlanElementInstance[] = [];
    constructor(private store: Store<fromCasePlanInstance.CasePlanInstanceState>, private route: ActivatedRoute, private casePlanInstanceService: CasePlanInstanceService, private translateService: TranslateService, private snackBar: MatSnackBar) { }

    ngOnInit() {
        this.store.pipe(select(fromCasePlanInstance.selectGetResult)).subscribe((casePlanInstance : CasePlanInstance) => {
            if (!casePlanInstance) {
                return;
            }

            this.casePlanInstance = casePlanInstance;
            this.activeActivities = casePlanInstance.Elements.filter((cp) => {
                return cp.State == "Active";
            });
            this.enableActivities = casePlanInstance.Elements.filter((cp) => {
                return cp.State == "Enabled";
            });
            this.completeActivities = casePlanInstance.Elements.filter((cp) => {
                return cp.State == "Completed";
            });
        });
        this.refresh();
    }

    enable(casePlanElementInstance : CasePlanElementInstance) {
        this.casePlanInstanceService.enable(this.casePlanInstance.Id, casePlanElementInstance.Id).subscribe(() => {
            this.snackBar.open(this.translateService.instant('ACTIVITY_IS_ENABLED'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        }, () => {
            this.snackBar.open(this.translateService.instant('CANNOT_ENABLE_ACTIVITY'), this.translateService.instant('UNDO'), {
                duration: 2000
            });
        });
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let request = new StartGet(id);
        this.store.dispatch(request);
    }
}