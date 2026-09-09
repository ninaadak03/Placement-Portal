import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../../core/services/auth.service';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.css',
})
export class AdminLoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly toastService = inject(ToastService);

  protected readonly loginForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
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

    this.authService.adminLogin(request).subscribe({
      next: (response) => {
        this.isSubmitting.set(false);

        this.authService.saveAuth(response);

        this.router.navigate(['/admin']);
      },

      error: (error) => {
        this.isSubmitting.set(false);

        const message = error.error?.message ?? 'Login failed. Please check your credentials.';

        this.toastService.error(message);
      },
    });
  }
}
