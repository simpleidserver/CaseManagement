import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store } from '@ngrx/store';

@Component({
    selector: 'view-humantaskdef-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDef implements OnInit {
    humanTaskDef: HumanTaskDef = new HumanTaskDef();

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute,) {

    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
        });

        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }
}
