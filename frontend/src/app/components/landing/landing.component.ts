import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-landing',
  imports: [RouterLink],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css',
})
export class LandingComponent implements OnInit {
  private auth = inject(AuthService);
  private router = inject(Router);
  isLoggedIn: boolean = false;
  private subscription!: Subscription;

  ngOnInit(): void {
    // Suscripción al estado de autenticación
    this.subscription = this.auth.authStatus$.subscribe((status) => {
      this.isLoggedIn = !!status;
    });
    if (this.isLoggedIn == true) this.router.navigate(['/home']);
  }

  ngOnDestroy(): void {
    // Evitar fugas de memoria
    this.subscription.unsubscribe();
  }
}
