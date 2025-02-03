import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from "@angular/router";
import { map, Observable } from "rxjs";
import { AuthService } from "../services/auth.service";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.authService.authStatus$.pipe(
      map((isAuthenticated: any) => {
        if (!!isAuthenticated) {
          // Permite el acceso si el usuario está autenticado
          return true;
        } else {
          // Redirige al login si no esta autenticado
          this.router.navigate(["/login"]);
          return false;
        }
      })
    );
  }
}
