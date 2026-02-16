import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { map } from "rxjs";
import { AuthStateService } from "../services/auth-state/auth-state.service";

export const guestGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authState = inject(AuthStateService);

  return authState.ensureLoaded().pipe(
    map(user => user == null ? true : router.createUrlTree(["home"]))
  );
};
