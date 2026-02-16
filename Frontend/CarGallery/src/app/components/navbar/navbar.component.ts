import { Component } from '@angular/core';
import { Router} from "@angular/router";
import { AuthStateService } from "../../services/auth-state/auth-state.service";

@Component({
  selector: 'navbar',
  template: `<nav class="navbar navbar-expand-lg navbar-dark bg-black ">
      <div class="container">
        <a class="navbar-brand" routerLink="home">CarGallery</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                data-bs-target="#navbarNav" aria-controls="navbarNav"
                aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto mb-2 mb-lg-0">
            <li class="nav-item">
              <a class="nav-link" aria-current="page"
                 routerLink="home" routerLinkActive="active">Home</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" aria-current="page"
                 routerLink="all" routerLinkActive="active">All</a>
            </li>
            <li *ngIf="authState.isAdmin$ | async" class="nav-item">
              <a class="nav-link" aria-current="page"
                 routerLink="panel" routerLinkActive="active">
                <i class="fa-solid fa-lock" aria-current="page" routerLink="panel"></i>
                Administrator panel</a>
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
  constructor(private router: Router, public authState: AuthStateService) {}
  Logout(){
    this.authState.logout().subscribe(() => {
      this.router.navigate(['login'])
    })
  }
}
