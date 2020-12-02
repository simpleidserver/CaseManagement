import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'view-execution-pointer',
    templateUrl: './viewpointer.component.html',
    styleUrls: ['./viewpointer.component.scss']
})
export class ViewExecutionPointerComponent implements OnInit {
    id: string = '';
    pathid: string = '';
    eltid: string = '';

    constructor(
        private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.parent.parent.params.subscribe((p: any) => {
            this.id = p['id'];
        });
        this.route.parent.params.subscribe((p: any) => {
            this.pathid = p['pathid'];
        });
        this.route.params.subscribe((p: any) => {
            this.eltid = p['eltid'];
        });
    }
}
