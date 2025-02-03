import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from "@angular/core";
import { Task } from "../../models/task.model";
import { DatePipe, NgClass } from "@angular/common";
import { TaskService } from "../../services/task.service";

@Component({
  selector: "app-task",
  imports: [DatePipe, NgClass],
  templateUrl: "./task.component.html",
  styleUrl: "./task.component.css",
})
export class TaskComponent implements OnInit {
  private _task = inject(TaskService);
  @Input() task!: Task;
  @Output() deleteTask = new EventEmitter<number>();
  statusClass = "";

  ngOnInit(): void {
    this.statusClass = this.getStatusClass(this.task.statusId);
  }

  onDelete() {
    var resultado = window.confirm("Borrar tarea?");
    if (resultado) {
      this._task.deleteTask(this.task.id).subscribe({
        next: () => {
          this.deleteTask.emit(this.task.id);
          this._task.clearTaskToEDit();
        },
        error: (err) => {},
      });
    }
  }

  // No me gusta hardcodeado
  getStatusClass(statusId: number): any {
    switch (statusId) {
      case 1: // Pendiente
        return "bg-warning";
      case 2: // En proceso
        return "bg-secondary";
      case 3: // Finalizada
        return "bg-success";
      case 4: // Cancelada
        return "bg-danger";
      default: // Estado desconocido
        return "bg-light text-dark";
    }
  }

  onEdit() {
    this._task.setTaskToEdit(this.task);
  }
}
