import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from "../components/login/login.component";
import { HomeComponent } from "../components/home/home.component";
import { AllCarsComponent } from "../components/all-cars/all-cars.component";
import { SignupComponent } from "../components/signup/signup.component";
import { PanelComponent } from "../components/panel/panel.component";
import { authGuard } from "./auth.guard";
import { adminGuard } from "./admin.guard";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'signUp',
    component: SignupComponent
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [authGuard]
  },
  {
    path: 'all',
    component: AllCarsComponent
  },
  {
    path: 'panel',
    component: PanelComponent,
    canActivate: [adminGuard]
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class RoutingModule { }
