import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { map } from "rxjs";

export const hasRoleGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const roles = route.data?.["role"] as string;

  return auth.authStatus$.pipe(
    map((isAuthenticated: any) => {
      if (isAuthenticated == null) {
        // No esta auth.
        router.navigate(["/login"]);
        return false;
      }
      if (isAuthenticated.role != roles) {
        // No tiene el rol necesario
        router.navigate(["/home"]);
        return false;
      }
      // Auth y con rol necesario.
      return true;
    })
  );
};
