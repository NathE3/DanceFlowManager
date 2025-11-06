import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../service/Auth.service';
import { RegistroDTO } from '../../models/registroDTO';
import { firstValueFrom } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule]
})
export class RegisterComponent {
  username: string = '';
  name: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  apellido: string = '';
  telefono: number = 0;

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService ) {}

  async register() {
    if (!this.username || !this.email || !this.password || !this.confirmPassword) {
      this.toastr.warning('Todos los campos son obligatorios.', 'Aviso');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.toastr.error('Las contraseñas no coinciden.', 'Error');
      return;
    }

    const registroDto: RegistroDTO = {
      name: this.name,
      userName: this.username,
      email: this.email,
      password: this.password,
      apellido: this.username,
      telefono: this.telefono,
      isProfesor: false
    };

    try {
      await firstValueFrom(this.authService.register(registroDto));
      this.toastr.success('Usuario registrado con éxito', 'Éxito');
      this.router.navigate(['/']);
    } catch (error: any) {
      this.toastr.error(error.message || 'Ocurrió un error inesperado', 'Error');
    }
  }

  goToLogin() {
    this.router.navigate(['/']);
  }
}