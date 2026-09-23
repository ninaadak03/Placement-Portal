import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { StudentProfileResponseDto } from '../models/student/student-profile-response.dto';
import { StudentOpeningResponseDto } from '../models/student/student-opening-response.dto';
import { ServiceResponseDto } from '../models/auth/service-response.dto';
import { StudentApplicationResponseDto } from '../models/student/student-application-response.dto';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private readonly http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiBaseUrl}/student`;

  getProfile(): Observable<StudentProfileResponseDto> {
    return this.http.get<StudentProfileResponseDto>(`${this.baseUrl}/profile`);
  }

  getOpenings(): Observable<StudentOpeningResponseDto[]> {
    return this.http.get<StudentOpeningResponseDto[]>(`${this.baseUrl}/openings`);
  }

  getApplications(): Observable<StudentApplicationResponseDto[]> {
    return this.http.get<StudentApplicationResponseDto[]>(`${this.baseUrl}/applications`);
  }

  applyToOpening(openingId: number): Observable<ServiceResponseDto> {
    return this.http.post<ServiceResponseDto>(`${this.baseUrl}/openings/${openingId}/apply`, {});
  }
}
