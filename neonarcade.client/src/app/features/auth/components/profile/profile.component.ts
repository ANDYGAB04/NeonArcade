import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { User } from '../../../../models/user.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  standalone: false
})
export class ProfileComponent implements OnInit {
user: User | null = null;
isAdmin = false;
showLogoutModal = false;

constructor(
  private authService: AuthService,
  private router: Router
) {}

ngOnInit(): void {
  this.loadUserProfile();
}

loadUserProfile(): void {
  this.user = this.authService.getCurrentUser();
  this.isAdmin = this.authService.isAdmin();
}

logout(): void {
  this.showLogoutModal = true;
}

confirmLogout(): void {
  this.showLogoutModal = false;
  this.authService.logout();
}

cancelLogout(): void {
  this.showLogoutModal = false;
}

goToOrders(): void {
    this.router.navigate(['/orders']);
  }

  goToGames(): void {
    this.router.navigate(['/games']);
  }

  goToCart(): void {
    this.router.navigate(['/cart']);
  }
}
