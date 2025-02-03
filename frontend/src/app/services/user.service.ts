import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';
environment;
@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  userUrl: string = `${environment.apiUrl}users/`;
  constructor() {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.userUrl);
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.userUrl}/${userId}`);
  }

  deactivateUser(userId: number): Observable<User> {
    return this.http.put<User>(`${this.userUrl}${userId}/deactivate`, userId);
  }

  activateUser(userId: number): Observable<User> {
    return this.http.put<User>(`${this.userUrl}${userId}/reactivate`, userId);
  }

  updateRole(userId: number, roleName: string): Observable<User> {
    const data = { userId: userId, roleName: roleName };
    return this.http.put<User>(`${this.userUrl}${userId}/role`, data);
  }
}
