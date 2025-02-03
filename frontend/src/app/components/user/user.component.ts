import { DatePipe, NgClass } from '@angular/common';
import { Component, inject, Input } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { AuthService } from '../../services/auth.service';
DatePipe;
@Component({
  selector: 'app-user',
  imports: [DatePipe, NgClass],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
})
export class UserComponent {
  @Input() user!: User;
  private _auth = inject(AuthService);
  private _userService = inject(UserService);

  handleActivation(userId: number) {
    if (this.user.isActive) {
      // Lo da de baja
      this._userService.deactivateUser(this.user.id).subscribe({
        next: (res) => {
          this.user.isActive = res.isActive;
        },
        error: (err) => console.log(err),
      });
    } else {
      this._userService.activateUser(this.user.id).subscribe({
        next: (res) => {
          this.user.isActive = res.isActive;
        },
        error: (err) => console.log(err),
      });
    }
  }

  handleRole(userId: number) {
    if (this.user.roleName == 'Admin') {
      // Hacerlo User
      this._userService.updateRole(userId, 'User').subscribe({
        next: (res) => {
          this.user.roleName = res.roleName;
        },
        error: (err) => console.log(err),
      });
    } else {
      // Hacerlo Admin
      this._userService.updateRole(userId, 'Admin').subscribe({
        next: (res) => {
          this.user.roleName = res.roleName;
        },
        error: (err) => console.log(err),
      });
    }
  }
}
