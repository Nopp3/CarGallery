import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { SessionService } from "../../services/session/session.service";
import { CarService } from "../../services/car/car.service";
import { Brand, CarUI } from "../../models/car.model";

@Component({
  selector: 'app-all-cars',
  templateUrl: './all-cars.component.html',
  styleUrls: ['./all-cars.component.css']
})
export class AllCarsComponent {
  constructor(private router: Router, private carService: CarService) {}

  cars: CarUI[] = []
  brands: Brand[] = []
  filter: number = 0

  ngOnInit(){
    if (SessionService.get("ActiveUser") == null){
      this.router.navigate(['login'])
    }
    this.carService.getAllCars()
      .subscribe({
        next: value => this.cars = value
      })
    this.carService.getUsedBrands()
      .subscribe({
        next: value => this.brands = value
      })
  }
  onFilterChange(){
    switch (this.filter){
      case 0:
        this.carService.getAllCars()
          .subscribe({
            next: value => this.cars = value
          })
        break;
      default:
        this.carService.getCarsByBrand(this.filter)
          .subscribe({
            next: value => this.cars = value
          })
        break;
    }
  }
}
