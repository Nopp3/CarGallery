import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { CarUI } from "../../models/car.model";
import { CarService } from "../../services/car/car.service";
import { MatDialog } from "@angular/material/dialog";
import { AddCarComponent } from "../car-form/add-car.component";
import { EditCarComponent } from "../car-form/edit-car.component";
import { environment } from 'src/app/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  constructor(private router : Router, private carService: CarService,
              private dialog: MatDialog) {}
  cars: CarUI[] = []
  apiUrl: string = environment.baseApiUrl
  ngOnInit(){
    this.carService.getMyCars()
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
    this.dialog.open(EditCarComponent, {
      width: '50%',
      data: {
        carGuid: id,
      }
    })
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
