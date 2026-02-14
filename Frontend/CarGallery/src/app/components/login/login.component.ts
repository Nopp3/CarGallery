import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { Login } from "../../models/user.model";
import { UserService } from "../../services/user/user.service";
import { SessionService } from "../../services/session/session.service";
import { SharedService } from "../../services/shared/shared.service";

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
  displayMessageBox = false;
  messageBoxText = "";
  constructor(private userService: UserService, private router: Router,
              private sharedService : SharedService) { }
  ngOnInit(){
    if (SessionService.get("ActiveUser") != null){
      this.router.navigate(['home'])
    }
    this.displayMessageBox = false;
  }
  loginUser(){
    SessionService.clear()
    this.userService.loginUser(this.loginRequest)
      .subscribe({
        next: (auth) => {
          SessionService.set("ActiveUser", auth.userId)
          this.sharedService.emitRefreshEvent()
          this.router.navigate(['home'])
        },
        error: (response) => {
          if (this.loginRequest.username != '' &&
              this.loginRequest.password != '') {
            if (response.status == 401) {
              this.messageBoxText = "Invalid username or password";
            } else if (typeof response.error === "string" && response.error.length > 0) {
              this.messageBoxText = response.error;
            } else {
              this.messageBoxText = "Login failed";
            }
            this.displayMessageBox = true;
          }
        }
      })
  }
}
