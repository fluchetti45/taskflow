import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NgIf } from '@angular/common';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-profile',
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private subscription!: Subscription;
  private fb = inject(FormBuilder);
  isLoggedIn: boolean = false;
  hasError: boolean = false;
  loggedIn: boolean = false;
  form: FormGroup;

  constructor() {
    this.form = this.fb.group({
      email: [''],
      id: [''],
      createdAt: [''],
      isActive: [''],
      roleName: [''],
    });
  }

  ngOnInit(): void {
    this.subscription = this.authService.authStatus$.subscribe((status) => {
      this.isLoggedIn = !!status;
    });
    if (this.isLoggedIn == false) this.router.navigate(['login']);
    this.authService.getUserData().subscribe({
      next: (res) => {
        this.form.patchValue(res);
      },
      error: (err) => {
        this.hasError = true;
      },
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  logout() {
    this.authService.logout();
  }

  deactivateAccount() {
    const id: number = Number(this.form.controls['id'].value);
    this.userService.deactivateUser(id).subscribe({
      next: (res) => console.log(res),
      error: (err) => console.log(err),
    });
    this.authService.logout();
  }
}
