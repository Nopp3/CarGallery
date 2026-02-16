import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthStateService } from "../services/auth-state/auth-state.service";
import { map } from "rxjs";

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authState = inject(AuthStateService);
  return authState.ensureLoaded().pipe(
    map(user => {
      if (user == null) return router.createUrlTree(["login"]);
      const isAdmin = user.role === "HeadAdmin" || user.role === "Admin";
      return isAdmin ? true : router.createUrlTree(["home"]);
    })
  );
};
