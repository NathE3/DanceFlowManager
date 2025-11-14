import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { LoginDTO } from '../models/loginDTO';
import { RegistroDTO } from '../models/registroDTO';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  readonly baseUrl = 'https://localhost:7777/DanceFlowApi/User';
  private loginUrl = `${this.baseUrl}/login`;
  private registerUrl = `${this.baseUrl}/register`;

  private token: string | null = null;

  /** 🔔 Estado reactivo del login */
  private loggedIn$ = new BehaviorSubject<boolean>(this.hasToken());

  /** Observable público para los componentes */
  isLoggedIn$ = this.loggedIn$.asObservable();

  constructor(private toastr: ToastrService) {
    // Recuperar token almacenado al iniciar
    this.token = localStorage.getItem('authToken');
  }

  /** Comprueba si hay token guardado */
  private hasToken(): boolean {
    return !!localStorage.getItem('authToken');
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
            this.loggedIn$.next(true); 
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
      const parts = token.split('.');
      if (parts.length !== 3) {
        this.toastr.warning('Token con formato inválido');
        return null;
      }

      const payload = parts[1];
      const decodedPayload = atob(payload);
      const parsedPayload = JSON.parse(decodedPayload);

      const id = parsedPayload.id ?? parsedPayload.sub ?? null;

      return id ? String(id) : null;
    } catch {
      this.toastr.error('Error al decodificar el token');
      return null;
    }
  }

  /** 🚪 Cierra sesión */
  logout(): void {
    this.token = null;
    localStorage.removeItem('authToken');
    this.loggedIn$.next(false); 
  }


  get isLoggedIn(): boolean {
    return this.loggedIn$.value;
  }
}
