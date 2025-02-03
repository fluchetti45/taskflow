import { Component, inject } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { LoginUser } from "../../models/login.model";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { Subscription } from "rxjs";
import { NgIf } from "@angular/common";
@Component({
  selector: "app-login",
  imports: [ReactiveFormsModule, RouterLink, NgIf],
  templateUrl: "./login-form.component.html",
  styleUrl: "./login-form.component.css",
})
export class LoginComponent {
  form: FormGroup;
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  private auth = inject(AuthService);
  isLoading: boolean = false;
  hasError: boolean = false;
  errorMsg: string = "";
  isLoggedIn: boolean = false;
  private subscription!: Subscription;

  constructor() {
    this.subscription = this.auth.authStatus$.subscribe((status) => {
      this.isLoggedIn = !!status;
    });
    if (this.isLoggedIn) {
      this.router.navigate(["/profile"]);
    }
    this.form = this.formBuilder.group({
      email: ["", [Validators.required, Validators.email]],
      password: ["", Validators.required],
    });
  }

  onSubmit(): void {
    this.hasError = false;
    this.errorMsg = "";
    if (this.form.valid) {
      this.isLoading = true;
      const loginUser: LoginUser = this.form.value;
      this.auth.login(loginUser).subscribe({
        next: (response) => {
          this.router.navigate(["/home"]);
          this.isLoading = false;
        },
        error: (error) => {
          this.hasError = true;
          this.isLoading = false;
          if (error.status == 0) {
            this.errorMsg = "Error en el servidor. Volve a intentar luego.";
          } else {
            this.errorMsg = error.error.message;
          }
        },
      });
    } else {
      this.hasError = true;
      this.errorMsg = "Completa el formulario.";
    }
  }
}
