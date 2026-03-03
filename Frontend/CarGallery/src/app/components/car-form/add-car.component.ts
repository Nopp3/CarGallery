import { Component } from '@angular/core';
import { Body, Brand, Car, Fuel } from "../../models/car.model";
import { CarService } from "../../services/car/car.service";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'app-car-form',
  templateUrl: './car-form.component.html',
  styleUrls: ['./car-form.component.css']
})
export class AddCarComponent {
  private readonly maxImageBytes = 5 * 1024 * 1024
  title: string = 'Add Car'
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
              private dialogRef: MatDialogRef<AddCarComponent>) {}
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
  }
  addEditCar(){
    if (this.carRequest.fuel_id == 0 || this.carRequest.body_id == 0 || this.carRequest.brand_id == 0 || !this.fileRequest){
      this.displayMessageBox = true
      this.messageBoxText = 'Something was not selected'
    }
    else{
      let formData = new FormData()
      formData.append('carRequest', JSON.stringify(this.carRequest))
      formData.append('image', this.fileRequest, this.fileRequest.name)

      this.carService.addCar(formData)
        .subscribe({
          next: (x) => {
            window.location.reload()
            this.dialogRef.close()
          },
          error: err => {
            this.displayMessageBox = true
            this.messageBoxText = this.getUploadErrorMessage(err)
          }
        })
    }
  }
  getCurrentYear(): number {
    return new Date().getFullYear()
  }
  fileSelected(event: any){
    const file = event.target.files[0]
    if (!file){
      this.fileRequest = null
      return
    }

    if (file.size > this.maxImageBytes){
      this.fileRequest = null
      event.target.value = ''
      this.displayMessageBox = true
      this.messageBoxText = 'Image file is too large. Maximum size is 5 MB.'
      return
    }

    this.displayMessageBox = false
    this.fileRequest = file
  }

  private getUploadErrorMessage(err: any): string {
    if (err?.status === 413){
      return 'Image file is too large. Maximum size is 5 MB.'
    }

    if (err?.status === 0){
      return 'Upload was interrupted or rejected before the API could process it.'
    }

    return err?.error ?? err?.message ?? 'Upload failed.'
  }
}
