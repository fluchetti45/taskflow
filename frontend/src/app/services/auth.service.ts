import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { LoginUser } from '../models/login.model';
import { Router } from '@angular/router';
import { SignupUser } from '../models/signup.model';
import { jwtDecode } from 'jwt-decode';
import { UserToken } from '../models/userToken.model';
import { MyJwtPayload } from '../models/myJwtPayload.models';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private url: string = environment.apiUrl;
  private authStatus = new BehaviorSubject<UserToken | null>(this.loadUser());
  public loggedIn: boolean = false;
  public authStatus$ = this.authStatus.asObservable();

  private loadUser(): UserToken | null {
    const token = localStorage.getItem('token');
    if (!token) {
      return null;
    }
    const decodedToken = jwtDecode<MyJwtPayload>(token);
    return {
      role: decodedToken.role,
      id: decodedToken.id,
      email: decodedToken.email,
    };
  }

  public isLoggedIn(): boolean {
    return !!this.authStatus.value;
  }

  userToken(): string | null {
    return localStorage.getItem('token');
  }

  login(user: LoginUser): Observable<any> {
    return this.http.post(`${this.url}auth/login`, user).pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem('token', response.token); // Guardar el token en el localStorage
          const user = jwtDecode<MyJwtPayload>(response.token);
          this.authStatus.next(user); // Actualizar el estado de autenticación
          this.loggedIn = true;
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.authStatus.next(null);
    this.loggedIn = false;
    this.router.navigate(['/login']);
  }

  signup(user: SignupUser): Observable<any> {
    return this.http.post(`${this.url}auth/signup`, user);
  }

  getUserData(): Observable<User> {
    return this.http.get<User>(`${this.url}users/${this.authStatus.value?.id}`);
  }
}
