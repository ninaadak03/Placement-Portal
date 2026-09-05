import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';
import { AuthTab } from '../../../core/models/auth/auth-tab.enum';

@Component({
  selector: 'app-otp',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './otp.component.html',
  styleUrl: './otp.component.css',
})
export class OtpComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toastService = inject(ToastService);

  protected readonly otpForm = this.fb.nonNullable.group({
    otpCode: ['', [Validators.required, Validators.maxLength(10)]],
  });

  protected readonly isSubmitting = signal(false);
  protected readonly serverError = signal('');

  protected readonly email: string = history.state?.email ?? '';

  protected onSubmit(): void {
    if (this.otpForm.invalid || this.isSubmitting()) {
      this.otpForm.markAllAsTouched();
      return;
    }

    const registration = this.authService.getPendingRegistration();

    if (!registration) {
      this.router.navigate(['/']);
      return;
    }

    this.isSubmitting.set(true);
    this.serverError.set('');

    const request = {
      email: registration.email,
      rollNo: registration.rollNo,
      password: registration.password,
      otpCode: this.otpForm.controls.otpCode.value,
    };

    this.authService.verifyOtp(request).subscribe({
      next: () => {
        this.isSubmitting.set(false);

        this.authService.clearPendingRegistration();

        this.toastService.success('Account created successfully. You can now log in.');

        this.router.navigate(['/'], {
          state: {
            activeTab: AuthTab.StudentLogin,
          },
        });
      },

      error: (error) => {
        this.isSubmitting.set(false);

        const message = error.error?.message ?? 'OTP verification failed. Please try again.';

        this.serverError.set(message);
        this.toastService.error(message);
      },
    });
  }
}
