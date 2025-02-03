import { Component, inject, OnInit } from "@angular/core";
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from "../../services/auth.service";
import { NgIf } from "@angular/common";
import { Subscription } from "rxjs";

@Component({
  selector: "app-navbar",
  imports: [RouterLink, NgIf],
  templateUrl: "./navbar.component.html",
  styleUrl: "./navbar.component.css",
})
export class NavbarComponent implements OnInit {
  auth = inject(AuthService);
  isLoggedIn: boolean = false;
  userRole: string = "";
  private subscription!: Subscription;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    // Suscripción al estado de autenticación
    this.subscription = this.authService.authStatus$.subscribe((status) => {
      this.isLoggedIn = !!status;
      if (status != null) {
        this.userRole = status["role"];
      }
    });
  }

  onLogout(): void {
    const confirmed = window.confirm(
      "¿Estás seguro de que quieres cerrar sesión?"
    );
    if (confirmed) {
      this.authService.logout();
    }
  }

  ngOnDestroy(): void {
    // Evitar fugas de memoria
    this.subscription.unsubscribe();
  }
}
