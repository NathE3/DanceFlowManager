import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../service/Auth.service';
import { LoginDTO } from '../../models/loginDTO';
import { firstValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService} from 'ngx-toastr';


@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router,private toastr: ToastrService ) {}

  async login() {
    const loginDto: LoginDTO = {
      email: this.email, 
      password: this.password,
      token: ''
    };
    try {
      const response = await firstValueFrom(this.authService.login(loginDto));
      if (response?.token) {
        localStorage.setItem('token', response.token);
        this.toastr.success('Has iniciado sesión correctamente', 'Éxito');
        this.router.navigate(['/principal']);
      } else {
       this.toastr.error('Usuario o contraseña incorrectos.', 'Error');
      }      
    } catch (error: any) {
     this.toastr.error(error.message || 'Ocurrió un error inesperado', 'Error');
    }
  }

  goToRegister() {
    this.router.navigate(['/registro']);
  }
}