import { Component, inject, OnInit } from '@angular/core';
import { UserComponent } from '../user/user.component';
import { NgFor, NgIf } from '@angular/common';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-dashboard',
  imports: [UserComponent, NgFor, NgIf],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  private userService = inject(UserService);
  users: User[] = [];
  hasError: boolean = false;
  isLoading: boolean = true;
  errorMsg: string = '';

  ngOnInit(): void {
    this.userService.getUsers().subscribe({
      next: (response) => {
        this.users = response;
        this.isLoading = false;
      },
      error: (err) => {
        this.hasError = true;
        this.isLoading = false;
        this.errorMsg = err.message;
      },
    });
  }
}
