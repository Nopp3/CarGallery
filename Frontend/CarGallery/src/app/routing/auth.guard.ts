import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthStateService } from "../services/auth-state/auth-state.service";
import { map } from "rxjs";

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authState = inject(AuthStateService);

  return authState.ensureLoaded().pipe(
    map(user => user != null ? true : router.createUrlTree(["login"]))
  );
};
