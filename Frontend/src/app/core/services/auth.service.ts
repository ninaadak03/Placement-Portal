import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { StudentRegisterRequest } from '../models/auth/student-register-request.model';
import { ServiceResponseDto } from '../models/auth/service-response.dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/auth`;

  register(request: StudentRegisterRequest): Observable<ServiceResponseDto> {
    return this.http.post<ServiceResponseDto>(`${this.baseUrl}/register`, request);
  }
}
