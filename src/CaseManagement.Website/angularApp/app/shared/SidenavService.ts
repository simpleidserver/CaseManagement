import { Injectable } from "@angular/core";
import { MatSidenav } from "@angular/material";

@Injectable()
export class SidenavService {
    private sidenav: MatSidenav;

    public setSidnav(sidnav: MatSidenav) {
        this.sidenav = sidnav;
    }

    public open() {
        this.sidenav.open();
    }

    public close() {
        this.sidenav.close();
    }

    public toggle() {
        this.sidenav.toggle();
    }
}