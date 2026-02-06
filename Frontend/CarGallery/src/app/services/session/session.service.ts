import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private static readonly DotNetRoleClaimType = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

  static set(key: string, value: any): void {
    sessionStorage.setItem(key, value);
  }
  static get(key: string): any {
    return sessionStorage.getItem(key);
  }
  static getAccessToken(): string | null {
    return sessionStorage.getItem('AccessToken');
  }
  static getRole(): string | null {
    const token = this.getAccessToken();
    if (!token) return null;

    try {
      const payload = jwtDecode<Record<string, any>>(token);
      const roleValue = payload['role'] ?? payload[this.DotNetRoleClaimType];
      return typeof roleValue === 'string' ? roleValue : null;
    } catch {
      return null;
    }
  }
  static remove(key: string): void {
    sessionStorage.removeItem(key);
  }
  static clear(): void {
    sessionStorage.clear();
  }
}
