import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { SessionService } from "../../services/session/session.service";
import { CarService } from "../../services/car/car.service";
import { CarUI } from "../../models/car.model";

@Component({
  selector: 'app-all-cars',
  templateUrl: './all-cars.component.html',
  styleUrls: ['./all-cars.component.css']
})
export class AllCarsComponent {
  constructor(private router: Router, private carService: CarService) {}

  cars: CarUI[] = []

  ngOnInit(){
    if (SessionService.get("ActiveUser") == null){
      this.router.navigate(['login'])
    }
    this.carService.getAllCars()
      .subscribe({
        next: value => this.cars = value
      })
  }
}
