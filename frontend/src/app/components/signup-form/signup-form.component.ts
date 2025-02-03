import { Component, inject } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { RouterLink } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";
import { SignupUser } from "../../models/signup.model";
import { NgIf } from "@angular/common";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-signup",
  imports: [ReactiveFormsModule, RouterLink, NgIf],
  templateUrl: "./signup-form.component.html",
  styleUrl: "./signup-form.component.css",
})
export class SignupComponent {
  private formBuilder = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  isLoading: boolean = false;
  hasError: boolean = false;
  errorMessage: string = "";
  signupForm: FormGroup;

  constructor() {
    this.signupForm = this.formBuilder.group({
      email: ["", [Validators.required, Validators.email]],
      password1: ["", [Validators.required]],
      password2: ["", [Validators.required]],
    });
  }

  validatePasswords(p1: string, p2: string): boolean {
    if (p1 === p2) return true;
    return false;
  }

  onSubmit() {
    this.hasError = false;
    this.errorMessage = "";
    if (this.signupForm.valid) {
      this.isLoading = true;
      const signupUser: SignupUser = this.signupForm.value;
      if (this.validatePasswords(signupUser.password1, signupUser.password2)) {
        this.auth.signup(signupUser).subscribe({
          next: () => {
            this.toastr.success("Cuenta creada!");
            this.isLoading = false;
            this.router.navigate(["/login"]);
          },
          error: (err) => {
            this.hasError = true;
            this.errorMessage = err.error.message;
            this.isLoading = false;
          },
        });
      } else {
        this.hasError = true;
        this.errorMessage = "Las contraseñas no coinciden.";
      }
    } else {
      this.hasError = true;
      this.errorMessage = "Formulario invalido.";
    }
  }
}
