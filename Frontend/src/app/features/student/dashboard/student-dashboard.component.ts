import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentService } from '../../../core/services/student.service';
import { StudentOpeningResponseDto } from '../../../core/models/student/student-opening-response.dto';
import { StudentApplicationResponseDto } from '../../../core/models/student/student-application-response.dto';

enum OpeningTab {
  Active = 'Active',
  Ineligible = 'Ineligible',
  Applied = 'Applied',
}

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-dashboard.component.html',
  styleUrl: './student-dashboard.component.css',
})
export class StudentDashboardComponent {
  private readonly studentService = inject(StudentService);

  protected readonly OpeningTab = OpeningTab;

  protected readonly studentName = signal('');
  protected readonly isLoading = signal(true);
  protected readonly isOpeningsLoading = signal(true);

  protected readonly selectedTab = signal<OpeningTab>(OpeningTab.Active);

  protected readonly openings = signal<StudentOpeningResponseDto[]>([]);

  protected readonly applications = signal<StudentApplicationResponseDto[]>([]);
  protected readonly isApplicationsLoading = signal(true);

  constructor() {
    this.loadProfile();
    this.loadOpenings();
    this.loadApplications();
  }

  protected selectTab(tab: OpeningTab): void {
    this.selectedTab.set(tab);
  }

  protected get activeOpenings(): StudentOpeningResponseDto[] {
    return this.openings().filter((opening) => opening.isEligible && !opening.hasApplied);
  }

  protected get ineligibleOpenings(): StudentOpeningResponseDto[] {
    return this.openings().filter((opening) => !opening.isEligible);
  }

  private loadApplications(): void {
    this.studentService.getApplications().subscribe({
      next: (applications) => {
        this.applications.set(applications);
        this.isApplicationsLoading.set(false);
      },
      error: () => {
        this.isApplicationsLoading.set(false);
      },
    });
  }

  private loadProfile(): void {
    this.studentService.getProfile().subscribe({
      next: (profile) => {
        this.studentName.set(profile.name);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      },
    });
  }

  private loadOpenings(): void {
    this.studentService.getOpenings().subscribe({
      next: (openings) => {
        this.openings.set(openings);
        this.isOpeningsLoading.set(false);
      },
      error: () => {
        this.isOpeningsLoading.set(false);
      },
    });
  }
}
