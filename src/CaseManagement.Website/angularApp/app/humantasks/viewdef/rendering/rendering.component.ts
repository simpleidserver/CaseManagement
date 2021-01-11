import { Component, ViewEncapsulation } from '@angular/core';
import { GuidGenerator } from './guidgenerator';

@Component({
    selector: 'view-humantaskdef-rendering-component',
    templateUrl: './rendering.component.html',
    styleUrls: ['./rendering.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDefRenderingComponent {
    option: any = {
        type: 'container',
        children: []
    };

    constructor() { }

    dragColumn(evt: any) {
        const json: any = {
            id: GuidGenerator.newGUID(),
            type: 'row',
            children: [
                { id: GuidGenerator.newGUID(), type: 'column', width: '50%', children: [] },
                { id: GuidGenerator.newGUID(), type: 'column', width: '50%', children: [] }
            ]
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragTxt(evt: any) {
        const json = {
            id: GuidGenerator.newGUID(),
            type: 'txt',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragSelect(evt: any) {
        const json = {
            id: GuidGenerator.newGUID(),
            type: 'select',
            label: 'Label',
            name: 'name'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragHeader(evt: any) {
        const json = {
            id: GuidGenerator.newGUID(),
            type: 'header',
            txt: 'Header',
            class: 'mat-display-1'
        };
        evt.dataTransfer.setData('json', JSON.stringify(json));
    }

    dragOver(evt: any) {
        evt.preventDefault();
    }

    dropColumn(evt: any) {
        const json = JSON.parse(evt.dataTransfer.getData('json'));
        this.option.children.push(json);
    }
}