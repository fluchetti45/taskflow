import { DatePipe, TitleCasePipe } from "@angular/common";
import { Component, inject, Input } from "@angular/core";
import { Role } from "../../models/role.model";
import { RoleService } from "../../services/role.service";

@Component({
  selector: "app-role",
  imports: [DatePipe],
  templateUrl: "./role.component.html",
  styleUrl: "./role.component.css",
})
export class RoleComponent {
  @Input() role!: Role;

  private _roleService: RoleService = inject(RoleService);

  handleDelete(roleId: number) {
    this._roleService.deleteRole(roleId).subscribe({
      next: (res) => {},
      error: (err) => {},
    });
  }
}
