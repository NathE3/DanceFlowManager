import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { FooterComponent } from './component/footer/footer.component';
import { AuthService } from './service/Auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterModule, FooterComponent, CommonModule],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'Harteraphia';
  usuarioId: string = '';

  constructor(public authService: AuthService, private router: Router) {
    this.usuarioId = this.authService.getAlumnoIdFromToken() ?? '';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['']);
  }
}

