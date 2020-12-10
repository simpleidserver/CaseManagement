import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import { CmmnPlanInstanceResult, CmmnPlanItemInstanceResult } from '@app/stores/cmmninstances/models/cmmn-planinstance.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-cmmn-planinstance-elt',
    templateUrl: './viewelt.component.html',
    styleUrls: ['./viewelt.component.scss']
})
export class ViewCasePlanEltInstanceComponent implements OnInit {
    children: CmmnPlanItemInstanceResult[] = [];
    cmmnPlanInstance: CmmnPlanInstanceResult = null;
    id: string = null;

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,
        private router: Router) {
    }

    ngOnInit() {
        this.store.pipe(select(fromAppState.selectCmmnPlanInstanceResult)).subscribe((e: CmmnPlanInstanceResult) => {
            if (!e) {
                return;
            }

            this.cmmnPlanInstance = e;
            this.refresh();
        });
        this.route.params.subscribe(() => {
            this.refreshSelectedId();
            this.refresh();
        });
        this.refreshSelectedId();
    }

    refresh() {
        if (!this.cmmnPlanInstance) {
            return;
        }

        const eltId = this.route.snapshot.params['eltid'];
        this.children = this.cmmnPlanInstance.children.filter(function (r: CmmnPlanItemInstanceResult) {
            return r.eltId === eltId;
        }).sort((a: CmmnPlanItemInstanceResult, b: CmmnPlanItemInstanceResult) => {
            return b.nbOccurrence - a.nbOccurrence;
        });
    }

    refreshSelectedId() {
        if (this.route.children && this.route.children[0]) {            
            this.id = this.route.children[0].snapshot.params['instid'];
        }
    }

    navigate(evt: any, elt: CmmnPlanItemInstanceResult) {
        evt.preventDefault();
        let id = this.route.parent.snapshot.params['id'];
        this.id = elt.id;
        this.router.navigate(['/cmmns/cmmninstances/' + id + '/' + elt.eltId + '/' + elt.id + '/history']);
    }
}