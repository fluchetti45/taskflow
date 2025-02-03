import { HttpClient } from '@angular/common/http';
import { inject, Injectable, OnInit } from '@angular/core';
import { Role } from '../models/role.model';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private http = inject(HttpClient);
  private url: string = `${environment.apiUrl}roles/`;

  constructor() {}

  getRoles(): Observable<Role[]> {
    return this.http.get<Role[]>(this.url);
  }

  createRole(roleName: string): Observable<Role> {
    const data = { roleName: roleName };
    return this.http.post<Role>(this.url, data, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  deleteRole(roleId: number) {
    return this.http.delete(`${this.url}${roleId}`);
  }
}
