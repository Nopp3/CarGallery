import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { CarUI, Car, Body, Brand, Fuel } from "../../models/car.model";

import { environment } from "../../environment";

@Injectable({
  providedIn: 'root'
})
export class CarService {
  baseApiUrl: string = environment.baseApiUrl;
  constructor(private http: HttpClient) { }
  getCarByGuid(guid: string): Observable<Car>{
    return this.http.get<Car>(this.baseApiUrl + '/Cars?id=' + guid)
  }
  getAllCars(): Observable<CarUI[]>{
    return this.http.get<CarUI[]>(this.baseApiUrl + '/Cars/all')
  }
  getMyCars(): Observable<CarUI[]>{
    return this.http.get<CarUI[]>(this.baseApiUrl + '/Cars/me')
  }
  getUserCars(userId: string): Observable<CarUI[]>{
    return this.http.get<CarUI[]>(this.baseApiUrl + '/Cars/user?id=' + userId)
  }
  getCarsByBrand(brandIdRequest: number): Observable<CarUI[]> {
    return this.http.get<CarUI[]>(this.baseApiUrl + '/Cars/brand?id=' + brandIdRequest)
  }
  addCar(formDataRequest: FormData): Observable<Car>{
    return this.http.post<Car>(this.baseApiUrl + '/Cars', formDataRequest)
  }
  updateCar(formDataRequest: FormData, guid: string): Observable<Car>{
    return this.http.put<Car>(this.baseApiUrl + '/Cars/' + guid, formDataRequest)
  }
  deleteCar(guid: string): Observable<Car>{
    return this.http.delete<Car>(this.baseApiUrl + '/Cars?id=' + guid)
  }

  getFuels(): Observable<Fuel[]>{
    return this.http.get<Fuel[]>(this.baseApiUrl + '/Cars/Fuels')
  }
  getBodies(): Observable<Body[]>{
    return this.http.get<Body[]>(this.baseApiUrl + '/Bodies')
  }
  getBrands(): Observable<Brand[]>{
    return this.http.get<Brand[]>(this.baseApiUrl + '/Brands')
  }
  getUsedBrands(): Observable<Brand[]>{
    return this.http.get<Brand[]>(this.baseApiUrl + '/Brands/used')
  }
  addBody(bodyRequest: Body): Observable<Body>{
    return this.http.post<Body>(this.baseApiUrl + '/Bodies', bodyRequest)
  }
  addBrand(brandRequest: Brand): Observable<Brand>{
    return this.http.post<Brand>(this.baseApiUrl + '/Brands', brandRequest)
  }
  deleteBody(id: number): Observable<Body>{
    return this.http.delete<Body>(this.baseApiUrl + '/Bodies/?id=' + id)
  }
  deleteBrand(id: number): Observable<Brand>{
    return this.http.delete<Brand>(this.baseApiUrl + '/Brands/?id=' + id)
  }
}
