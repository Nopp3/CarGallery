import { Component } from '@angular/core';
import { User } from "../../models/user.model";
import {UserService} from "../../services/user/user.service";
import {Router} from "@angular/router";

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
  constructor(private userService: UserService, private router: Router) {}
  ngOnInit(){
    this.displayMessageBox = false;
  }
  addUser(){
    if(this.confirmPass == this.newUser.password){
      this.userService.addUser(this.newUser)
        .subscribe({
          next: () => {
            this.router.navigate(['/login']);
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
