import { Component, Inject } from '@angular/core';
import { Body, Brand, Car, Fuel } from "../../models/car.model";
import { CarService } from "../../services/car/car.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'app-car-form',
  templateUrl: './car-form.component.html',
  styleUrls: ['./car-form.component.css']
})
export class EditCarComponent {
  title: string = 'Edit Car'
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
  fileRequest: File | null = null;

  brandsToSelect: Brand[] = []
  bodiesToSelect: Body[] = []
  fuelsToSelect: Fuel[] = []
  displayMessageBox = false
  messageBoxText = ""
  constructor(private carService: CarService,
              private dialogRef: MatDialogRef<EditCarComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) {}
  ngOnInit(){
    this.displayMessageBox = false;

    this.carService.getBrands().subscribe({
      next: x => this.brandsToSelect = x
    })
    this.carService.getBodies().subscribe({
      next: x => this.bodiesToSelect = x
    })
    this.carService.getFuels().subscribe({
      next: x => this.fuelsToSelect = x
    })
    this.carService.getCarByGuid(this.data.carGuid).subscribe({
      next: x => this.carRequest = x
    })
  }
  addEditCar(){
    if (this.carRequest.fuel_id == 0 || this.carRequest.body_id == 0 || this.carRequest.brand_id == 0){
      this.displayMessageBox = true
      this.messageBoxText = 'Something was not selected'
    }
    else{
      let formData = new FormData()
      formData.append('updateCarRequest', JSON.stringify(this.carRequest))

      if (this.fileRequest){
        formData.append('image', this.fileRequest, this.fileRequest.name)
      }

      this.carService.updateCar(formData, this.data.carGuid)
        .subscribe({
          next: (x) => {
            window.location.reload()
            this.dialogRef.close()
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
  fileSelected(event: any){
    this.fileRequest = event.target.files[0]
  }
}
