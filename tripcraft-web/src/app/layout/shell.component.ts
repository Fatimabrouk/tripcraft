import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  template: `
    <div style="display: flex;">
      <app-sidebar></app-sidebar>
      <main style="flex: 1; padding: 1.5rem;">
        <router-outlet></router-outlet>
      </main>
    </div>
  `
})
export class ShellComponent {}