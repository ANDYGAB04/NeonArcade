import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';
import { Router } from '@angular/router';
import { LoginRequest, RegisterRequest, AuthResponse, User } from '../../models/user.model';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private storageService: StorageService,
    private router: Router
  ) {
    this.loadUserFromStorage();
  }

  /**
   * Load user from localStorage on service initialization
   */
  private loadUserFromStorage(): void {
    const user = this.storageService.getUser();
    if (user) {
      this.currentUserSubject.next(user);
    }
  }

  /**
   * Login user with email and password
   */
  login(credentials: LoginRequest): Observable<any> {
    return this.http.post<any>('/login?useCookies=false&useSessionCookies=false', credentials)
      .pipe(
        tap(response => {
          // Identity API returns tokenType, accessToken, expiresIn, refreshToken
          if (response.accessToken) {
            this.storageService.setToken(response.accessToken);
            // Create user object from credentials since Identity API doesn't return user info
            const user: User = {
              id: '',
              userName: credentials.email.split('@')[0], // Extract username from email
              email: credentials.email,
              roles: []
            };
            this.storageService.setUser(user);
            this.currentUserSubject.next(user);
          }
        })
      );
  }

  /**
   * Register a new user
   */
  register(data: RegisterRequest): Observable<any> {
    return this.http.post<any>('/register', {
      email: data.email,
      password: data.password
    }).pipe(
      tap(() => {
        // After successful registration, automatically login
        this.login({ email: data.email, password: data.password }).subscribe();
      })
    );
  }

  /**
   * Logout user and clear stored data
   */
  logout(): void {
    this.storageService.clear();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return !!this.storageService.getToken();
  }

  /**
   * Check if current user has admin role
   */
  isAdmin(): boolean {
    const user = this.currentUserSubject.value;
    return user?.roles?.includes('Admin') ?? false;
  }

  /**
   * Get current logged-in user
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  /**
   * Get current user as observable
   */
  getCurrentUser$(): Observable<User | null> {
    return this.currentUser$;
  }
}
