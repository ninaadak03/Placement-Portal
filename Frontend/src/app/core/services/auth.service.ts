import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { StudentRegisterRequest } from '../models/auth/student-register-request.model';
import { ServiceResponseDto } from '../models/auth/service-response.dto';
import { VerifyOtpRequestDto } from '../models/auth/verify-otp-request.dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/auth`;

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
}
