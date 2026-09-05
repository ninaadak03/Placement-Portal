import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-student-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './student-register.component.html',
  styleUrl: './student-register.component.css',
})
export class StudentRegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toastService = inject(ToastService);

  protected readonly registerForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
    rollNo: [
      '',
      [
        Validators.required,
        Validators.minLength(11),
        Validators.maxLength(12),
        Validators.pattern(/^(01|02)NC(22|23)(CS|IS|EC|EE|EI|ME|CV|BT)(?!000)\d{3}$/),
      ],
    ],
    password: [
      '',
      [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$/),
      ],
    ],
  });

  protected readonly isSubmitting = signal(false);

  protected onSubmit(): void {
    if (this.registerForm.invalid || this.isSubmitting()) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);

    const request = this.registerForm.getRawValue();

    this.authService.register(request).subscribe({
      next: () => {
        this.isSubmitting.set(false);

        this.authService.setPendingRegistration(request);

        this.router.navigate(['/verify-otp'], {
          state: {
            email: request.email,
          },
        });
      },

      error: (error) => {
        this.isSubmitting.set(false);

        const message = error.error?.message ?? 'Registration failed. Please try again.';

        this.toastService.error(message);
      },
    });
  }
}
