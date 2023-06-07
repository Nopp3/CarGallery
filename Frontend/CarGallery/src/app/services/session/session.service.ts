import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private static sessionData: any = {};

  static set(key: string, value: any): void {
    this.sessionData[key] = value;
  }

  static get(key: string): any {
    return this.sessionData[key];
  }

  static remove(key: string): void {
    delete this.sessionData[key];
  }

  static clear(): void {
    this.sessionData = {};
  }
}
