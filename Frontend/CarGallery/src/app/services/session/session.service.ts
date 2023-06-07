import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private sessionData: any = {};

  set(key: string, value: any): void {
    this.sessionData[key] = value;
  }

  get(key: string): any {
    return this.sessionData[key];
  }

  remove(key: string): void {
    delete this.sessionData[key];
  }

  clear(): void {
    this.sessionData = {};
  }
}
