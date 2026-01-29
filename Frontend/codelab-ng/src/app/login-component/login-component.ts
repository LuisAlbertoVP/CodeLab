import { Component, inject, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CodelabButton } from '../shared/components/codelab-button/codelab-button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginService } from './login-service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-component',
  imports: [MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule, CodelabButton, ReactiveFormsModule],
  templateUrl: './login-component.html'
})
export class LoginComponent {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly router = inject(Router);
  private readonly service = inject(LoginService);
  private readonly snackBar = inject(MatSnackBar);

  ocultar = signal(true);
  isLoading = signal(false);

  form = this.fb.group({
    correo: ['', Validators.required],
    clave: ['', Validators.required],
  });

  iniciarSesion() {
    if (this.form.invalid) {
      this.snackBar.open('Por favor, complete todos los campos requeridos.', 'Cerrar');
      return;
    }
    this.isLoading.set(true);
    const form = this.form.value;
    this.service.iniciarSesion(form.correo!, form.clave!).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (error: HttpErrorResponse) => {
        console.error('Error al iniciar sesión:', error);
        this.snackBar.open('Credenciales incorrectas.', 'Cerrar');
        this.isLoading.set(false);
      }
    });
  }
}