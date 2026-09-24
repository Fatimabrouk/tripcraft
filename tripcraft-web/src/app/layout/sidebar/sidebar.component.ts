import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  template: `
    <nav class="sidebar">
      <div class="brand">TripCraft</div>
      <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
      <a routerLink="/trips" routerLinkActive="active">My trips</a>
      <a routerLink="/explore" routerLinkActive="active">Explore</a>
      <a routerLink="/assistant" routerLinkActive="active">AI assistant</a>
      <a routerLink="/settings" routerLinkActive="active">Settings</a>
    </nav>
  `,
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {}