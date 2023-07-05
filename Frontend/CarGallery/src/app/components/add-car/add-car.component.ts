import { Component } from '@angular/core';
import { SessionService } from "../../services/session/session.service";
import { Router } from "@angular/router";
import { Body, Brand, Car, Fuel } from "../../models/car.model";
import { CarService } from "../../services/car/car.service";

@Component({
  selector: 'app-add-car',
  templateUrl: './add-car.component.html',
  styleUrls: ['./add-car.component.css']
})
export class AddCarComponent {
  carRequest: Car = {
    id: '00000000-0000-0000-0000-000000000000',
    user_id: '00000000-0000-0000-0000-000000000000',
    fuel_id: 0,
    body_id: 0,
    brand_id: 0,
    model:	'',
    productionYear: 0,
    engine: 0,
    horsePower: 0,
    imagePath: ''
  }
  brandsToSelect: Brand[] = []
  bodiesToSelect: Body[] = []
  fuelsToSelect: Fuel[] = []
  displayMessageBox = false
  messageBoxText = ""
  constructor(private carService: CarService, private router: Router) {}
  ngOnInit(){
    this.displayMessageBox = false;
    if (SessionService.get("ActiveUser") == null){
      this.router.navigate(['login'])
    }

    this.carService.getBrands().subscribe({
      next: x => this.brandsToSelect = x
    })
    this.carService.getBodies().subscribe({
      next: x => this.bodiesToSelect = x
    })
    this.carService.getFuels().subscribe({
      next: x => this.fuelsToSelect = x
    })
  }
  addCar(){
    this.carRequest.user_id = SessionService.get("ActiveUser")
    if (this.carRequest.fuel_id == 0 || this.carRequest.body_id == 0 || this.carRequest.brand_id == 0){
      this.displayMessageBox = true
      this.messageBoxText = 'Something was not selected'
    }
    else{
      this.carService.addCar(this.carRequest)
        .subscribe({
          next: (x) => {
            console.log('success')
            console.log(x)
          },
          error: err => {
            this.displayMessageBox = true
            this.messageBoxText = err.message
          }
        })
    }
  }
  getCurrentYear(): number {
    return new Date().getFullYear()
  }
}
