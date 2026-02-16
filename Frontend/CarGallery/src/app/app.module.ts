import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RoutingModule } from "./routing/routing.module";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { MatDialogModule } from "@angular/material/dialog";

import { AppComponent } from './app.component';
import { NavbarComponent } from "./components/navbar/navbar.component";
import { LoginComponent } from "./components/login/login.component";
import { HomeComponent } from "./components/home/home.component";
import { AllCarsComponent } from "./components/all-cars/all-cars.component";
import { SignupComponent } from "./components/signup/signup.component";
import { PanelComponent } from "./components/panel/panel.component";
import { PanelFormComponent } from "./components/panel/panel-form.component";
import { AddCarComponent } from "./components/car-form/add-car.component";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditCarComponent } from "./components/car-form/edit-car.component";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    HomeComponent,
    AllCarsComponent,
    SignupComponent,
    PanelComponent,
    PanelFormComponent,
    AddCarComponent,
    EditCarComponent
  ],
  imports: [
    BrowserModule,
    RoutingModule,
    FormsModule,
    HttpClientModule,
    MatDialogModule,
    BrowserAnimationsModule
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
