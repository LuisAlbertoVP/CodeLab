import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private http = inject(HttpClient);
  token: string | null = null;
  
  iniciarSesion(correo: string, clave: string) {
    const headers = { Authorization: 'Basic ' + btoa(`${correo}:${clave}`) };
    return this.http.get<{ token: string }>('/Auth/IniciarSesion', { headers, withCredentials: true })
      .pipe(
        tap(respuesta => this.token = respuesta.token),
        map(response => response.token)
      );
  }
}