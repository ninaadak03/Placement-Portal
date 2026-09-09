import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { StudentRegisterRequest } from '../models/auth/student-register-request.model';
import { ServiceResponseDto } from '../models/auth/service-response.dto';
import { VerifyOtpRequestDto } from '../models/auth/verify-otp-request.dto';
import { StudentLoginRequestDto } from '../models/auth/student-login-request.dto';
import { AdminLoginRequestDto } from '../models/auth/admin-login-request.dto';
import { AuthResponseDto } from '../models/auth/auth-response.dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/auth`;

  private readonly tokenKey = 'placement_portal_token';
  private readonly roleKey = 'placement_portal_role';
  private readonly profileCompletedKey = 'placement_portal_profile_completed';

  private pendingRegistration: StudentRegisterRequest | null = null;

  register(request: StudentRegisterRequest): Observable<ServiceResponseDto> {
    return this.http.post<ServiceResponseDto>(`${this.baseUrl}/register`, request);
  }

  setPendingRegistration(request: StudentRegisterRequest): void {
    this.pendingRegistration = request;
  }

  getPendingRegistration(): StudentRegisterRequest | null {
    return this.pendingRegistration;
  }

  clearPendingRegistration(): void {
    this.pendingRegistration = null;
  }

  verifyOtp(request: VerifyOtpRequestDto): Observable<ServiceResponseDto> {
    return this.http.post<ServiceResponseDto>(`${this.baseUrl}/verify-otp`, request);
  }

  studentLogin(request: StudentLoginRequestDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/student-login`, request);
  }

  adminLogin(request: AdminLoginRequestDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/admin-login`, request);
  }

  saveAuth(response: AuthResponseDto): void {
    if (!response.token || !response.role) {
      return;
    }

    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.roleKey, response.role);
    localStorage.setItem(this.profileCompletedKey, String(response.isProfileCompleted));
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getRole(): string | null {
    return localStorage.getItem(this.roleKey);
  }

  isProfileCompleted(): boolean {
    return localStorage.getItem(this.profileCompletedKey) === 'true';
  }

  isLoggedIn(): boolean {
    const token = this.getToken();

    if (!token) {
      return false;
    }

    try {
      const payload = token.split('.')[1];

      const decodedPayload = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));

      const expiryTime = decodedPayload.exp;

      if (!expiryTime || Date.now() >= expiryTime * 1000) {
        this.logout();
        return false;
      }

      return true;
    } catch {
      this.logout();
      return false;
    }
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.roleKey);
    localStorage.removeItem(this.profileCompletedKey);
  }
}
