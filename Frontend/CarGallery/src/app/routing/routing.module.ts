import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {LoginComponent} from "../components/login/login.component";
import {HomeComponent} from "../components/home/home.component";
import {AllCarsComponent} from "../components/all-cars/all-cars.component";
import {SignupComponent} from "../components/signup/signup.component";

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'all',
    component: AllCarsComponent
  },
  {
    path: 'signUp',
    component: SignupComponent
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class RoutingModule { }
