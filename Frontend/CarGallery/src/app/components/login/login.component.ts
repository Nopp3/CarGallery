import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { Login } from "../../models/user.model";
import { UserService } from "../../services/user/user.service";
import { AuthStateService } from "../../services/auth-state/auth-state.service";

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
              private authState: AuthStateService) { }
  ngOnInit(){
    this.authState.ensureLoaded().subscribe(user => {
      if (user != null) {
        this.router.navigate(['home'])
      }
    })
    this.displayMessageBox = false;
  }
  loginUser(){
    this.userService.loginUser(this.loginRequest)
      .subscribe({
        next: (auth) => {
          this.authState.setFromLogin(auth)
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
