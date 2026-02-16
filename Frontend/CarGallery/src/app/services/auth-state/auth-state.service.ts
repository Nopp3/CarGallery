import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, catchError, finalize, map, of, shareReplay, tap } from "rxjs";
import { AuthResponse, AuthUserResponse } from "../../models/user.model";
import { UserService } from "../user/user.service";

@Injectable({
  providedIn: "root"
})
export class AuthStateService {
  private readonly userSubject = new BehaviorSubject<AuthUserResponse | null>(null);
  private hasLoaded = false;
  private loadInFlight$?: Observable<AuthUserResponse | null>;

  user$ = this.userSubject.asObservable();
  isLoggedIn$ = this.user$.pipe(map(user => user != null));
  isAdmin$ = this.user$.pipe(map(user => user?.role === "HeadAdmin" || user?.role === "Admin"));

  constructor(private userService: UserService) {}

  ensureLoaded(): Observable<AuthUserResponse | null> {
    if (this.hasLoaded) {
      return of(this.userSubject.value);
    }
    if (this.loadInFlight$) {
      return this.loadInFlight$;
    }

    this.loadInFlight$ = this.userService.me().pipe(
      tap(user => {
        this.userSubject.next(user);
        this.hasLoaded = true;
      }),
      catchError(() => {
        this.userSubject.next(null);
        this.hasLoaded = true;
        return of(null);
      }),
      finalize(() => {
        this.loadInFlight$ = undefined;
      }),
      shareReplay(1)
    );

    return this.loadInFlight$;
  }

  setFromLogin(auth: AuthResponse) {
    this.userSubject.next({
      userId: auth.userId,
      username: auth.username,
      role: auth.role
    });
    this.hasLoaded = true;
  }

  clear() {
    this.userSubject.next(null);
    this.hasLoaded = true;
  }

  logout(): Observable<void> {
    return this.userService.logout().pipe(finalize(() => this.clear()));
  }
}
