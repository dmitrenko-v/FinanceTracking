import { CanActivateFn, Router } from "@angular/router";
import { inject } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { UserData } from "../UserData";

export const authenticatedGuard: CanActivateFn = () => {
  const router = inject(Router);
  const userObservable = inject(AuthService).user;
  let userData: UserData | undefined;
  userObservable.subscribe((user) => (userData = user));
  return Boolean(userData) || router.navigateByUrl("/login");
};
