import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { Car, Body, Brand } from "../../models/car.model";

import { environment } from "../../enviroment";

@Injectable({
  providedIn: 'root'
})
export class CarService {
  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient) { }



  getBodies(): Observable<Body[]>{
    return this.http.get<Body[]>(this.baseApiUrl+'/api/Bodies')
  }
  getBrands(): Observable<Brand[]>{
    return this.http.get<Brand[]>(this.baseApiUrl+'/api/Brands')
  }
  addBody(bodyRequest: Body): Observable<Body>{
    return this.http.post<Body>(this.baseApiUrl+'/api/Bodies', bodyRequest)
  }
  addBrand(brandRequest: Brand): Observable<Brand>{
    return this.http.post<Brand>(this.baseApiUrl+'/api/Brands', brandRequest)
  }
  deleteBody(id: number): Observable<Body>{
    return this.http.delete<Body>(this.baseApiUrl + '/api/Bodies/?id=' + id)
  }
  deleteBrand(id: number): Observable<Brand>{
    return this.http.delete<Brand>(this.baseApiUrl + '/api/Brands/?id=' + id)
  }
}
