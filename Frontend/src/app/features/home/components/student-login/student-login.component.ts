import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-student-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './student-login.component.html',
  styleUrl: './student-login.component.css',
})
export class StudentLoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toastService = inject(ToastService);

  protected readonly loginForm = this.fb.nonNullable.group({
    rollNo: [
      '',
      [
        Validators.required,
        Validators.minLength(11),
        Validators.maxLength(12),
        Validators.pattern(/^(01|02)NC(22|23)(CS|IS|EC|EE|EI|ME|CV|BT)(?!000)\d{3}$/),
      ],
    ],
    password: ['', [Validators.required]],
  });

  protected readonly isSubmitting = signal(false);

  protected onSubmit(): void {
    if (this.loginForm.invalid || this.isSubmitting()) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);

    const request = this.loginForm.getRawValue();

    this.authService.studentLogin(request).subscribe({
      next: (response) => {
        this.isSubmitting.set(false);

        this.authService.saveAuth(response);

        if (response.isProfileCompleted) {
          this.router.navigate(['/student']);
          return;
        }

        this.router.navigate(['/student/complete-profile']);
      },

      error: (error) => {
        this.isSubmitting.set(false);

        const message = error.error?.message ?? 'Login failed. Please check your credentials.';

        this.toastService.error(message);
      },
    });
  }
}
