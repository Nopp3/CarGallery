import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from "../components/login/login.component";
import { HomeComponent } from "../components/home/home.component";
import { AllCarsComponent } from "../components/all-cars/all-cars.component";
import { SignupComponent } from "../components/signup/signup.component";
import { PanelComponent } from "../components/panel/panel.component";

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
    component: HomeComponent
  },
  {
    path: 'all',
    component: AllCarsComponent
  },
  {
    path: 'panel',
    component: PanelComponent
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class RoutingModule { }
