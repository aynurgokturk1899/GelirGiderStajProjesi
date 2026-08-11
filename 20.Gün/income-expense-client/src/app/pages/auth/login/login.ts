import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { ApiProblemDetails } from '../../../core/models/auth.models';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  protected readonly isSubmitting = signal(false);
  protected readonly errorMessage = signal('');
  protected readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    password: ['', [Validators.required, Validators.maxLength(100)]],
  });

  protected submit(): void {
    this.errorMessage.set('');
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSubmitting.set(true);
    this.authService.login(this.form.getRawValue()).pipe(
      finalize(() => this.isSubmitting.set(false)),
    ).subscribe({
      next: () => void this.router.navigateByUrl(this.route.snapshot.queryParamMap.get('returnUrl') ?? '/dashboard'),
      error: (error: HttpErrorResponse) => this.errorMessage.set(this.getErrorMessage(error)),
    });
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    const problem = error.error as ApiProblemDetails | null;
    return problem?.detail ?? 'Sunucuya ulaşılamadı. Backend uygulamasının çalıştığını kontrol ediniz.';
  }
}
