import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {SessionService} from "../../services/session/session.service";
import {SharedService} from "../../services/shared/shared.service";

@Component({
  selector: 'navbar',
  template: `<nav class="navbar navbar-expand-lg navbar-dark bg-black ">
      <div class="container">
        <a class="navbar-brand" routerLink="">CarGallery</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                data-bs-target="#navbarNav" aria-controls="navbarNav"
                aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto mb-2 mb-lg-0">
            <li class="nav-item">
              <a class="nav-link active" aria-current="page"
                 routerLink="">Home</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" aria-current="page"
                 routerLink="all">All</a>
            </li>
          </ul>
          <button class="btn btn-light" aria-current="page"
             (click)="Logout()">Logout</button>
        </div>
      </div>
    </nav>`
})
export class NavbarComponent {
  title = 'Navbar';
  constructor(private router : Router, private sharedService : SharedService) {}
  Logout(){
    SessionService.clear()
    this.sharedService.emitRefreshEvent()
    this.router.navigate(['login'])
  }
}
