import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-student-header',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './student-header.component.html',
  styleUrl: './student-header.component.css',
})
export class StudentHeaderComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected isProfileCompleted(): boolean {
    return this.authService.isProfileCompleted();
  }

  protected logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
