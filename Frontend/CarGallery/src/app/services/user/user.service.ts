import { Injectable } from '@angular/core';

import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Login, User } from "../../models/user.model";

import { environment } from "../../enviroment";
@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient) { }

  getUsers() : Observable<User[]>{
    return this.http.get<User[]>(this.baseApiUrl + '/api/Users/all')
  }
  getUser(userGuid : string) : Observable<User>{
    return this.http.get<User>(this.baseApiUrl+'/api/Users/?id=' + userGuid)
  }
  loginUser(loginRequest: Login) : Observable<User>{
    return this.http.post<User>(this.baseApiUrl + '/api/Users/login', loginRequest)
  }
  addUser(addUserRequest: User): Observable<User>{
    addUserRequest.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<User>(this.baseApiUrl + '/api/Users', addUserRequest)
  }
}
