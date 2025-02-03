import { HttpClient } from "@angular/common/http";
import { inject, Injectable, OnInit } from "@angular/core";
import { BehaviorSubject, catchError, Observable } from "rxjs";
import { Status } from "../models/status.model";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: "root",
})
export class StatusService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}status/`;
  private statuses = new BehaviorSubject<Status[] | []>([]);
  public statuses$ = this.statuses.asObservable();

  getStatuses(): void {
    this.http
      .get<Status[]>(this.url)
      .pipe(
        catchError((error) => {
          return [];
        })
      )
      .subscribe((statuses) => {
        this.statuses.next(statuses);
      });
  }

  createStatus(statusName: string): Observable<Status> {
    return this.http.post<Status>(this.url, JSON.stringify(statusName), {
      headers: { "Content-Type": "application/json" },
    });
  }

  deleteStatus(statusId: number) {}
}
