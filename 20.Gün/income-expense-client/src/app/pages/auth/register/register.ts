import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { ApiProblemDetails } from '../../../core/models/auth.models';
import { AuthService } from '../../../core/services/auth.service';

function passwordsMatch(control: AbstractControl): ValidationErrors | null {
  return control.get('password')?.value === control.get('confirmPassword')?.value ? null : { passwordsMismatch: true };
}

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  protected readonly isSubmitting = signal(false);
  protected readonly errorMessage = signal('');
  protected readonly form = this.formBuilder.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(100), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)]],
    confirmPassword: ['', Validators.required],
  }, { validators: passwordsMatch });

  protected submit(): void {
    this.errorMessage.set('');
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.isSubmitting.set(true);
    this.authService.register(this.form.getRawValue()).pipe(
      finalize(() => this.isSubmitting.set(false)),
    ).subscribe({
      next: () => void this.router.navigate(['/dashboard']),
      error: (error: HttpErrorResponse) => this.errorMessage.set(this.getErrorMessage(error)),
    });
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    const problem = error.error as ApiProblemDetails | null;
    if (problem?.errors) return Object.values(problem.errors).flat()[0] ?? 'Bilgilerinizi kontrol ediniz.';
    return problem?.detail ?? 'Sunucuya ulaşılamadı. Backend uygulamasının çalıştığını kontrol ediniz.';
  }
}
