import { Component, Input } from '@angular/core';
import { CarService } from "../../services/car/car.service";
@Component({
  selector: 'panel-form',
  template: `
    <form ngNativeValidate #form="ngForm" (ngSubmit)="add()">
    <div class="input-group">
      <input class="form-control" style="width: 75%;" type="text"
             [(ngModel)]="stringToAdd" [ngModelOptions]="{standalone: true}" required>
      <button class="btn btn-dark fa-solid fa-plus" type="submit"></button>
    </div>
  </form>`,
})
export class PanelFormComponent {
  @Input() whichColumn: string = ''
  stringToAdd: string = ''
  constructor(private carService: CarService) {}
  add(){
    switch (this.whichColumn){
      case 'brands':
        this.carService.addBrand({id: 0, name: this.stringToAdd})
          .subscribe({
            next: () => {
              window.location.reload();
            }
          })
        break
      case 'bodies':
        this.carService.addBody({id: 0, type: this.stringToAdd})
          .subscribe({
            next: () => {
              window.location.reload();
            },
          })
        break
    }
  }
}
