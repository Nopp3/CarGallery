import { Component } from '@angular/core';
import { SessionService } from "../../services/session/session.service";
import { Router } from "@angular/router";
import { CarUI } from "../../models/car.model";
import { CarService } from "../../services/car/car.service";
import { MatDialog } from "@angular/material/dialog";
import { AddCarComponent } from "../add-car/add-car.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private router : Router, private carService: CarService,
              private dialog: MatDialog) {}
  cars: CarUI[] = []
  ngOnInit(){
    if (SessionService.get("ActiveUser") == null){
      this.router.navigate(['login'])
    }
    this.carService.getUserCars()
      .subscribe({
        next: value => this.cars = value
      })
  }
  addCar(){
    this.dialog.open(AddCarComponent, {
      width: '50%',
    })
  }
  editCar(id: string){

  }
  deleteCar(id: string){
    this.carService.deleteCar(id)
      .subscribe({
        next: () => {
          window.location.reload()
        }
      })
  }
}
