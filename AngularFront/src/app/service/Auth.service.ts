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

  private token: string | null = localStorage.getItem('authToken');

  private loggedIn$ = new BehaviorSubject<boolean>(!!this.token);

  isLoggedIn$ = this.loggedIn$.asObservable();

  constructor(private toastr: ToastrService) {}

  /** LOGIN */
  login(credentials: LoginDTO): Observable<any> {
    return new Observable<any>(observer => {
      fetch(this.loginUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(credentials)
      })
        .then(response => response.json())
        .then(data => {

          if (data?.token) {
            this.setToken(data.token);
            this.loggedIn$.next(true);
          } else {
            this.toastr.warning('⚠️ No se recibió un token válido');
          }

          observer.next(data);
          observer.complete();
        })
        .catch(error => observer.error(error));
    });
  }

  /** REGISTRO */
  register(registroDto: RegistroDTO): Observable<any> {
    return new Observable<any>(observer => {
      fetch(this.registerUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(registroDto)
      })
        .then(async response => {
          const data = await response.json();

          if (response.ok) {
            observer.next(data);
            observer.complete();
          } else {
            observer.error(new Error(data?.message || 'Error en el registro'));
          }
        })
        .catch(error => observer.error(error));
    });
  }

  /** Guarda el token */
  setToken(token: string): void {
    this.token = token;
    localStorage.setItem('authToken', token);
  }

  /** Obtiene el token */
  getToken(): string | null {
    return this.token || localStorage.getItem('authToken');
  }

getAlumnoIdFromToken(): string | null {
  const token = this.getToken(); 
  if (!token) return null;

  try {
    const payload = token.split('.')[1];
    if (!payload) return null;

    // Base64Url -> Base64 estándar
    let base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
    // Añadir padding si falta
    while (base64.length % 4) {
      base64 += '=';
    }

    const decoded = atob(base64);
    const json = JSON.parse(decoded);

    console.log('Token decodificado:', json); 
    return json.nameid ?? null;

  } catch (error) {
    console.error('Error decodificando token:', error);
    this.toastr.error('Error al decodificar token');
    return null;
  }
}



  /** LOGOUT */
  logout(): void {
    this.token = null;
    localStorage.removeItem('authToken');
    this.loggedIn$.next(false);
  }

  /** Getter práctico */
  get isLoggedIn(): boolean {
    return this.loggedIn$.value;
  }
}
