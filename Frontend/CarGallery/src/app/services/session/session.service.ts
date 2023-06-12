import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  static set(key: string, value: any): void {
    sessionStorage.setItem(key, value);
  }
  static get(key: string): any {
    return sessionStorage.getItem(key);
  }
  static remove(key: string): void {
    sessionStorage.removeItem(key);
  }
  static clear(): void {
    sessionStorage.clear();
  }
}
