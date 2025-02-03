import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { Task } from "../models/task.model";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: "root",
})
export class TaskService {
  private http = inject(HttpClient);
  private taskUrl = `${environment.apiUrl}task/`;
  private taskEditSubject = new BehaviorSubject<Task | null>(null);
  taskEditSubject$ = this.taskEditSubject.asObservable();

  constructor() {}

  setTaskToEdit(task: Task) {
    this.taskEditSubject.next(task);
  }

  clearTaskToEDit() {
    this.taskEditSubject.next(null);
  }

  getTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.taskUrl);
  }

  createTask(
    description: string,
    statusId: number,
    title: string
  ): Observable<Task> {
    const data = { Description: description, statusId: statusId, Title: title };
    return this.http.post<Task>(this.taskUrl, data, {
      headers: { "Content-Type": "application/json" },
    });
  }

  editTask(
    description: string,
    statusId: number,
    taskId: number,
    title: string
  ): Observable<Task> {
    const data = {
      Id: taskId,
      Description: description,
      StatusId: statusId,
      Title: title,
    };
    return this.http.put<Task>(`${this.taskUrl}${taskId}`, data);
  }

  deleteTask(id: number): Observable<any> {
    return this.http.delete(`${this.taskUrl}${id}`, { observe: "response" });
  }
}
