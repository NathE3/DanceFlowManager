import {Component} from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { FooterComponent } from './component/footer/footer.component';
import { AuthService } from './service/Auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterModule,FooterComponent],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'Harteraphia';

  constructor(public authService: AuthService) {}
}
