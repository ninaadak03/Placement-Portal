import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AuthTab } from '../../../../core/models/auth/auth-tab.enum';

@Component({
  selector: 'app-auth-tabs',
  standalone: true,
  imports: [],
  templateUrl: './auth-tabs.component.html',
  styleUrl: './auth-tabs.component.css',
})
export class AuthTabsComponent {
  @Input() selectedTab!: AuthTab;

  @Output() selectedTabChange = new EventEmitter<AuthTab>();

  protected readonly AuthTab = AuthTab;

  selectTab(tab: AuthTab): void {
    if (tab !== this.selectedTab) {
      this.selectedTabChange.emit(tab);
    }
  }
}
