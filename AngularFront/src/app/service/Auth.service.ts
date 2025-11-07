import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginDTO } from '../models/loginDTO';
import { RegistroDTO } from '../models/registroDTO';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  readonly baseUrl = 'https://localhost:7777/DanceFlowApi/users';
  private loginUrl = `${this.baseUrl}/userLogin`;
  private registerUrl = `${this.baseUrl}/userRegister`;
  private token: string | null = null;
  private loggedIn = false;

  constructor(private toastr: ToastrService ) {
    // Recuperar el token desde localStorage al inicializar el servicio
    this.token = localStorage.getItem('authToken');
  }

  /** 🔑 LOGIN */
login(credentials: LoginDTO): Observable<any> {
  return new Observable<any>(observer => {
    fetch(this.loginUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(credentials)
    })
      .then(response => response.json())
      .then(data => {
        console.log('Login response:', data);
        if (data?.result?.token) {
          this.setToken(data.result.token);
          this.loggedIn = true; // 🔹 mover aquí
        } else {
          this.toastr.warning('⚠️ No se recibió un token válido:', data);
        }
        observer.next(data);
        observer.complete();
      })
      .catch(error => observer.error(error));
  });
}



  /** 📝 REGISTRO */
  register(registroDto: RegistroDTO): Observable<any> {
    return new Observable<any>(observer => {
      fetch(this.registerUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(registroDto)
      })
        .then(async response => {
          const data = await response.json();
          this.toastr.success('Registro - Respuesta de la API:', data);

          if (response.ok) {
            observer.next(data);
            observer.complete();
          } else {
            observer.error(new Error(data?.message || 'Error en el registro, compruebe los campos'));
          }
        })
        .catch(error => observer.error(error));
    });
  }

  /** 💾 Guarda el token */
  setToken(token: string): void {
    this.token = token;
    localStorage.setItem('authToken', token);
  }

  /** 🔍 Obtiene el token actual */
  getToken(): string | null {
    return this.token || localStorage.getItem('authToken');
  }

  /** 🧩 Decodifica el ID del usuario desde el token */
  getAlumnoIdFromToken(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = token.split('.')[1];
      const decodedPayload = atob(payload);
      const parsedPayload = JSON.parse(decodedPayload);
      return parsedPayload.id || parsedPayload.sub || null;
    } catch (error) {
      this.toastr.error('Error al decodificar el token');
      return null;
    }
  }

  /** 🚪 Cierra sesión */
  logout(): void {
    this.token = null;
    localStorage.removeItem('authToken');
    this.loggedIn = false;
  }

  /** ✅ Comprueba si el usuario está logueado */
  get isLoggedIn(): boolean {
    return this.loggedIn;
  }
}
