import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {LoginComponent} from "../components/login/login.component";
import {HomeComponent} from "../components/home/home.component";
import {AllCarsComponent} from "../components/all-cars/all-cars.component";

const routes: Routes = [
  {
    path: '',
    component: LoginComponent
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
    path: 'logout',
    component: LoginComponent //To do
  },
  {
    path: 'signIn',
    component: LoginComponent //To do
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class RoutingModule { }
