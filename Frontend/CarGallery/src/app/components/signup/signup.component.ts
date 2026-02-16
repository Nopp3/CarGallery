import { Component } from '@angular/core';
import { Login, User } from "../../models/user.model";
import {UserService} from "../../services/user/user.service";
import {Router} from "@angular/router";
import { AuthStateService } from "../../services/auth-state/auth-state.service";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  newUser: User = {
    id: '',
    role_id: 2,
    username: '',
    email: '',
    password: ''
  }
  confirmPass = '';
  displayMessageBox = false;
  messageBoxText = "";
  constructor(private userService: UserService, private router: Router,
              private authState: AuthStateService) {}
  ngOnInit(){
    this.displayMessageBox = false;
  }
  addUser(){
    if(this.confirmPass == this.newUser.password){
      this.userService.addUser(this.newUser)
        .subscribe({
          next: () => {
            const loginRequest: Login = {
              username: this.newUser.username,
              password: this.newUser.password
            };
            this.userService.loginUser(loginRequest)
              .subscribe({
                next: auth => {
                  this.authState.setFromLogin(auth);
                  this.router.navigate(['/home']);
                },
                error: () => {
                  this.router.navigate(['/login']);
                }
              });
          },
          error: err => {
            this.messageBoxText = err.error;
            this.displayMessageBox = true;
          }
        })
    }else{
      this.messageBoxText = 'Passwords are different';
      this.displayMessageBox = true;
    }

  }
}
