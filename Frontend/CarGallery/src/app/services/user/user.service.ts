import { Injectable } from '@angular/core';

import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { AuthResponse, AuthUserResponse, Login, User } from "../../models/user.model";

import { environment } from "../../environment";
@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient) { }

  getUsers() : Observable<User[]>{
    return this.http.get<User[]>(this.baseApiUrl + '/Users/all')
  }
  getUser(userGuid : string) : Observable<User>{
    return this.http.get<User>(this.baseApiUrl+'/Users/?id=' + userGuid)
  }
  loginUser(loginRequest: Login) : Observable<AuthResponse>{
    return this.http.post<AuthResponse>(this.baseApiUrl + '/auth/login', loginRequest)
  }
  me() : Observable<AuthUserResponse>{
    return this.http.get<AuthUserResponse>(this.baseApiUrl + '/auth/me')
  }
  logout() : Observable<void>{
    return this.http.post<void>(this.baseApiUrl + '/auth/logout', {})
  }
  addUser(addUserRequest: User): Observable<User>{
    addUserRequest.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<User>(this.baseApiUrl + '/Users', addUserRequest)
  }
}
