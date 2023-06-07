import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { Login } from "../../models/user.model";
import { UserService } from "../../services/user/user.service";
import { SessionService } from "../../services/session/session.service";
import {SharedService} from "../../services/shared/shared.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginRequest: Login = {
    username: '',
    password: ''
  }
  constructor(private userService: UserService, private router: Router,
              private sharedService : SharedService) { }

  loginUser(){
    SessionService.clear()
    this.userService.loginUser(this.loginRequest)
      .subscribe({
        next: (guid) => {
          SessionService.set("LoggedUser", guid)
          this.sharedService.emitRefreshEvent()
          this.router.navigate(['home'])
        },
        error: (response) => {
          console.log(response.statusText)
        }
      })
  }
}
