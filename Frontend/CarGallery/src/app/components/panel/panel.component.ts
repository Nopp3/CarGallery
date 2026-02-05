import { Component } from '@angular/core';
import { SessionService } from "../../services/session/session.service";
import { Router } from "@angular/router";
import { CarService } from "../../services/car/car.service";
import { Brand, Body } from "../../models/car.model";
import { UserService } from "../../services/user/user.service";
import { User } from "../../models/user.model";

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.css']
})
export class PanelComponent {
  brands: Brand[] = []
  bodies: Body[] = []
  users: User[] = []
  newBrand: string = ''
  newBody: string = ''
  constructor(private route: Router, private carService: CarService,
              private userService: UserService) {}
  ngOnInit(){
    const activeUser = SessionService.get('ActiveUser')
    if (activeUser == null){
      this.route.navigate(['login'])
      return
    }

    this.userService.getUser(activeUser)
      .subscribe({
        next: user => {
          const isAdmin = user.role_id == 1 || user.role_id == 2
          if (!isAdmin){
            this.route.navigate(['home'])
            return
          }

          this.carService.getBrands()
            .subscribe({
              next: value => {
                this.brands = value
              }
            })
          this.carService.getBodies()
            .subscribe({
              next: value => {
                this.bodies = value
              }
            })
          this.userService.getUsers()
            .subscribe({
              next: value => {
                this.users = value
              }
            })
        },
        error: () => {
          this.route.navigate(['login'])
        }
      })
  }
  deleteRow(table: string, id: number){
    switch (table){
      case 'brand':
        this.carService.deleteBrand(id).subscribe({
          next: () => {
            window.location.reload()
          }
        })
        break
      case 'body':
        this.carService.deleteBody(id).subscribe({
          next: () => {
            window.location.reload()
          }
        })
        break
    }
  }
}
