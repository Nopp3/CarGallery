import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { SessionService } from "../services/session/session.service";
import { UserService } from "../services/user/user.service";
import { catchError, map, of } from "rxjs";

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const userService = inject(UserService);

  return userService.me().pipe(
    map(authUser => {
      SessionService.set("ActiveUser", authUser.userId);
      const isAdmin = authUser.role === "HeadAdmin" || authUser.role === "Admin";
      return isAdmin ? true : router.createUrlTree(["home"]);
    }),
    catchError(error => {
      if (error?.status === 401 || error?.status === 403) {
        SessionService.clear();
        return of(router.createUrlTree(["login"]));
      }

      return of(router.createUrlTree(["home"]));
    })
  );
};
