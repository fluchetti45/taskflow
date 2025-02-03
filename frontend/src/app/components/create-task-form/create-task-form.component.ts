import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { Status } from "../../models/status.model";
import { Task } from "../../models/task.model";
import { TaskService } from "../../services/task.service";
import { Subscription } from "rxjs";
import { NgIf } from "@angular/common";

@Component({
  selector: "app-create-task-form",
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: "./create-task-form.component.html",
  styleUrl: "./create-task-form.component.css",
})
export class CreateTaskFormComponent implements OnInit, OnDestroy {
  //
  private fb = inject(FormBuilder);
  private _task = inject(TaskService);
  //
  @Output() taskEdited = new EventEmitter<Task>();
  @Output() taskCreated = new EventEmitter<Task>();
  @Input() statuses: Status[] = [];
  //
  taskForm: FormGroup;
  taskToEdit: Task | null = null;
  subscription: Subscription = new Subscription();
  formErrors: boolean = false;

  constructor() {
    this.taskForm = this.fb.group({
      title: ["", [Validators.required, Validators.maxLength(20)]],
      description: ["", [Validators.required, Validators.maxLength(255)]],
      status: [null, Validators.required],
    });
  }
  ngOnInit(): void {
    this.subscription = this._task.taskEditSubject$.subscribe((task) => {
      if (task) {
        this.taskToEdit = task;
        this.fillTaskForm(task); // Actualiza el formulario cada vez que task cambie
      } else {
        this.taskForm.reset(); // Resetea el formulario si task es null
      }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this._task.clearTaskToEDit();
  }

  resetTaskForm(): void {
    this.taskForm.setValue({
      title: "",
      description: "",
      status: this.statuses[0].id,
    });
    this.taskToEdit = null;
  }

  fillTaskForm(task: Task): void {
    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      status: task.statusId,
    });
  }

  onSubmit() {
    if (this.taskForm.valid) {
      const title: string = this.taskForm.controls["title"].value;
      const description: string = this.taskForm.controls["description"].value;
      const statusId: number = Number(this.taskForm.controls["status"].value);
      if (this.taskToEdit == null) {
        this._task.createTask(description, statusId, title).subscribe({
          next: (res) => {
            this.taskCreated.emit(res);
          },
        });
      } else {
        this._task
          .editTask(description, statusId, this.taskToEdit.id, title)
          .subscribe({
            next: (res) => {
              this.taskEdited.emit(res);
              this._task.clearTaskToEDit();
              //this.taskToEdit = null;
            },
            error: (err) => {
              this._task.clearTaskToEDit();
              //this.taskToEdit = null;
            },
          });
      }
      this.resetTaskForm();
      this.formErrors = false;
    } else {
      this.formErrors = true;
    }
  }
}
