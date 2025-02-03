import { Component, inject, OnDestroy, OnInit } from "@angular/core";
import { NgFor, NgIf } from "@angular/common";
import { Task } from "../../models/task.model";
import { TaskService } from "../../services/task.service";
import { FormBuilder, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { TaskComponent } from "../task/task.component";
import { ToastrService } from "ngx-toastr";
import { StatusService } from "../../services/status.service";
import { Status } from "../../models/status.model";
import { CreateTaskFormComponent } from "../create-task-form/create-task-form.component";

@Component({
  selector: "app-home",
  imports: [
    NgIf,
    ReactiveFormsModule,
    TaskComponent,
    NgFor,
    CreateTaskFormComponent,
  ],
  templateUrl: "./home.component.html",
  styleUrl: "./home.component.css",
})
export class HomeComponent implements OnInit {
  private _task = inject(TaskService);
  private _toastr = inject(ToastrService);
  private fb = inject(FormBuilder);
  private _statuses = inject(StatusService);
  //
  filterForm: FormGroup;
  tareas: Task[] = [];
  tareasFiltradas: Task[] = []; // Innecesario si uso BehaviourSubject en taskService.
  statuses: Status[] = [];
  selectedStatus = "";
  isLoggedIn: boolean = false;
  hasFilter: boolean = false;
  isLoading: boolean;
  //
  constructor() {
    this.isLoading = true;
    this.filterForm = this.fb.group({
      statusFilter: [null],
    });
  }

  ngOnInit(): void {
    // Cargar las tareas del usuario.
    this.isLoading = true;
    this._task.getTasks().subscribe({
      next: (response) => {
        this.tareas = response;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
      },
    });
    //
    this._statuses.statuses$.subscribe({
      next: (response) => {
        this.statuses = response;
      },
      error: (err) => {},
    });
    this._statuses.getStatuses();
  }

  handleDelete(taskId: number) {
    //
    this.tareas = this.tareas.filter((t) => t.id !== taskId);
    this._toastr.warning("Tarea borrada.");
  }

  handleTaskCreated(task: Task) {
    this.tareas.unshift(task);
    this._toastr.success("Tarea creada exitosamente!");
  }

  handleTaskEdit(task: Task) {
    // Necesito id de la tarea para editar.
    const index: number = this.tareas.findIndex((t) => t.id == task.id);
    if (index !== -1) {
      // Genera un nuevo objeto con los datos del que esta en el index y luego actualiza las prop desc y statuId
      this.tareas[index] = {
        ...this.tareas[index],
        description: task.description,
        statusId: task.statusId,
        title: task.title,
        statusName: this.statuses[task.statusId - 1].statusName,
      };
      this._toastr.success("Tarea editada exitosamente!");
    }
  }

  handleFilter() {
    if (this.filterForm.controls["statusFilter"].value !== null) {
      this.isLoading = true;
      this.hasFilter = true;
      const filterId: number = this.filterForm.controls["statusFilter"].value;
      this.selectedStatus = this.statuses[filterId - 1].statusName;
      this.tareasFiltradas = this.tareas.filter((t) => t.statusId == filterId);
      this.isLoading = false;
    }
  }

  handleResetFilter() {
    this.hasFilter = false;
    this.filterForm.controls["statusFilter"].setValue(null);
  }
}
