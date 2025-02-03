import { Component, inject, OnInit } from "@angular/core";
import { RoleService } from "../../services/role.service";
import { Role } from "../../models/role.model";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { RoleComponent } from "../role/role.component";

@Component({
  selector: "app-roles",
  imports: [ReactiveFormsModule, RoleComponent],
  templateUrl: "./roles.component.html",
  styleUrl: "./roles.component.css",
})
export class RolesComponent implements OnInit {
  private _fb = inject(FormBuilder);
  private _roleService = inject(RoleService);
  private _toaster = inject(ToastrService);
  loading = true;
  hasError: boolean = false;
  roles: Role[] = [];
  form: FormGroup;

  constructor() {
    this.form = this._fb.group({
      roleName: ["", Validators.required],
    });
  }

  ngOnInit(): void {
    this._roleService.getRoles().subscribe({
      next: (res) => {
        this.roles = res;
        this.loading = false;
      },
      error: (err) => {
        this.hasError = true;
        this.loading = false;
      },
    });
  }

  handleSubmit() {
    const roleName: string = this.form.controls["roleName"].value;
    this._roleService.createRole(roleName).subscribe({
      next: (res) => {
        this._toaster.success("Rol creado.");
        this.roles.push(res);
      },
      error: (err) => {
        this._toaster.error("Algo salio mal.");
      },
    });
  }
}
