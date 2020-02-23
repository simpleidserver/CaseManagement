import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { StartGet } from '../actions/caseplaninstance';
import { CasePlanInstance } from '../models/caseplaninstance.model';
import * as fromCasePlanInstance from '../reducers';

@Component({
    selector: 'view-case-instances',
    templateUrl: './view.component.html',  
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewCasePlanInstanceComponent implements OnInit {
    casePlanInstance: CasePlanInstance = new CasePlanInstance();

    constructor(private store: Store<fromCasePlanInstance.CasePlanInstanceState>, private route: ActivatedRoute) { }

    ngOnInit() {
        this.store.pipe(select(fromCasePlanInstance.selectGetResult)).subscribe((casePlanInstance : CasePlanInstance) => {
            if (!casePlanInstance) {
                return;
            }

            this.casePlanInstance = casePlanInstance;
        });
        this.refresh();
    }

    refresh() {
        var id = this.route.snapshot.params['id'];
        let request = new StartGet(id);
        this.store.dispatch(request);
    }
}