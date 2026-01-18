import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ClaseComponent } from '../../component/clase/clase.component'; 
import { ClaseDTO } from '../../models/claseDTO'; 
import { AuthService } from '../../service/Auth.service';
import { ObjetoService } from '../../service/objeto.service';
@Component({
  selector: 'app-clasesInscritas',
  standalone: true,
  imports: [CommonModule, ClaseComponent],
  templateUrl: './clasesInscritas.component.html',
  styleUrls: ['./clasesInscritas.component.css']
})
export class ClasesInscritasComponent implements OnInit {

  ListaClasesInscritas: ClaseDTO[] = [];

  constructor(
    private objetoService: ObjetoService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  async ngOnInit() {
    const usuarioId = this.authService.getAlumnoIdFromToken();

    if (!usuarioId) {
      this.toastr.warning('Usuario no logueado');
      this.ListaClasesInscritas = [];
      return;
    }

    try {
      this.ListaClasesInscritas = await this.objetoService.getClaseByUsuario(usuarioId);
    } catch (error) {
      this.toastr.error('Error al mostrar clases por usuario');
      console.error(error);
    }
  }
}

