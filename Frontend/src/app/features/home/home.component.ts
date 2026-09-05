import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AuthCardComponent } from './components/auth-card/auth-card.component';
import { AuthTabsComponent } from './components/auth-tabs/auth-tabs.component';
import { AuthTab } from '../../core/models/auth/auth-tab.enum';
import { StudentLoginComponent } from './components/student-login/student-login.component';
import { StudentRegisterComponent } from './components/student-register/student-register.component';
import { AdminLoginComponent } from './components/admin-login/admin-login.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    AuthCardComponent,
    AuthTabsComponent,
    StudentLoginComponent,
    StudentRegisterComponent,
    AdminLoginComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  protected readonly AuthTab = AuthTab;

  protected selectedTab: AuthTab = history.state?.activeTab ?? AuthTab.StudentRegister;

  protected onSelectedTabChange(tab: AuthTab): void {
    this.selectedTab = tab;
  }
}
