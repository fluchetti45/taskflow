import { Component, inject, OnInit } from "@angular/core";
import { Status } from "../../models/status.model";
import { StatusService } from "../../services/status.service";
import { DatePipe, NgIf } from "@angular/common";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { StatusComponent } from "../status/status.component";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-statuses",
  imports: [NgIf, ReactiveFormsModule, StatusComponent],
  templateUrl: "./statuses.component.html",
  styleUrl: "./statuses.component.css",
})
export class StatusesComponent implements OnInit {
  private _status = inject(StatusService);
  private _fb = inject(FormBuilder);
  private _toastr = inject(ToastrService);
  statuses: Status[] = [];
  loading: boolean = true;
  hasError: boolean = false;

  form: FormGroup;

  constructor() {
    this.form = this._fb.group({
      statusName: ["", Validators.required],
    });
  }

  handleSubmit() {
    const statusName: string = this.form.controls["statusName"].value;
    this._status.createStatus(statusName).subscribe({
      next: (res) => {
        this.statuses.push(res);
        this._toastr.success("Status creado");
      },
      error: (err) => {
        this._toastr.error("Algo salio mal");
      },
    });
  }

  ngOnInit(): void {
    this._status.statuses$.subscribe({
      next: (res) => {
        this.statuses = res;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.hasError = true;
      },
    });
    this._status.getStatuses();
  }
}
