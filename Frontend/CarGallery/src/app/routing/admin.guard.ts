import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { SessionService } from "../services/session/session.service";

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const activeUser = SessionService.get("ActiveUser");
  const accessToken = SessionService.getAccessToken();

  if (!activeUser || !accessToken) {
    return router.createUrlTree(["login"]);
  }

  const role = SessionService.getRole();
  const isAdmin = role === "HeadAdmin" || role === "Admin";

  if (!isAdmin) {
    return router.createUrlTree(["home"]);
  }

  return true;
};
