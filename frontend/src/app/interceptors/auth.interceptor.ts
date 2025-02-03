import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.userToken();
  if (token) {
    // Si el token esta presente se agrega al header Authorization
    const newReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
    return next(newReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status == 401) {
          auth.logout();
        }
        return throwError(() => new Error(error.message));
      })
    );
  } else {
    return next(req);
  }
};
