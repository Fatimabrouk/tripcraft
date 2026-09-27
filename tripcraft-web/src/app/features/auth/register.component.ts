import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <div class="auth-card">
      <h2>Create your account</h2>

      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <label>
          Display name
          <input type="text" formControlName="displayName" />
        </label>

        <label>
          Email
          <input type="email" formControlName="email" />
        </label>

        <label>
          Password
          <input type="password" formControlName="password" />
        </label>

        @if (errorMessage()) {
          <p class="error">{{ errorMessage() }}</p>
        }

        <button type="submit" [disabled]="form.invalid || loading()">
          {{ loading() ? 'Creating account...' : 'Create account' }}
        </button>
      </form>

      <p class="switch">Already have an account? <a routerLink="/login">Log in</a></p>
    </div>
  `,
  styles: [`
    .auth-card { max-width: 340px; margin: 4rem auto; display: flex; flex-direction: column; gap: 10px; }
    label { display: flex; flex-direction: column; gap: 4px; font-size: 13px; }
    input { padding: 8px 10px; border: 1px solid #ddd; border-radius: 6px; }
    .error { color: #d32f2f; font-size: 13px; }
    .switch { font-size: 13px; text-align: center; }
  `]
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.errorMessage.set(null);

    const { email, password, displayName } = this.form.getRawValue();
    this.auth.register(email!, password!, displayName!).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err?.error?.[0] ?? 'Could not create account.');
      }
    });
  }
}