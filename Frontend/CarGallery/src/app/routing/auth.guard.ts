import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { SessionService } from "../services/session/session.service";

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const activeUser = SessionService.get("ActiveUser");
  const accessToken = SessionService.getAccessToken();

  if (activeUser && accessToken) {
    return true;
  }

  return router.createUrlTree(["login"]);
};
