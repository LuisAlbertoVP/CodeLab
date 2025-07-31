import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { LoginService } from './login-service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-component',
  imports: [MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule, ReactiveFormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.scss'
})
export class LoginComponent {
  service = inject(LoginService);

  form = new FormGroup({
    correo: new FormControl<string | null>(null, Validators.required),
    clave: new FormControl<string | null>(null, Validators.required)
  });

  iniciarSesion() {
    if (this.form.valid) {
      const form = this.form.value;
      this.service.iniciarSesion(form.correo!, form.clave!).subscribe({
        next: () => alert('Bienvenido!'),
        error: (error: HttpErrorResponse) => alert(error.error)
      });
    }
  }
}