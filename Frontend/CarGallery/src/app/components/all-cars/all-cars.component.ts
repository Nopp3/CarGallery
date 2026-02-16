import { Component } from '@angular/core';
import { CarService } from "../../services/car/car.service";
import { Brand, CarUI } from "../../models/car.model";
import { environment } from 'src/app/environment';

@Component({
  selector: 'app-all-cars',
  templateUrl: './all-cars.component.html',
  styleUrls: ['./all-cars.component.css']
})
export class AllCarsComponent {
  constructor(private carService: CarService) {}

  cars: CarUI[] = []
  apiUrl: string = environment.baseApiUrl
  brands: Brand[] = []
  filter: number = 0

  ngOnInit(){
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
