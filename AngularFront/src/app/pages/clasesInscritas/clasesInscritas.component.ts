import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ClaseComponent } from 'src/app/component/clase/clase.component';
import { ClaseDTO } from 'src/app/models/claseDTO';
import { AuthService } from 'src/app/service/Auth.service';
import { ObjetoService } from 'src/app/service/objeto.service';

@Component(
{
  selector: 'app-clasesInscritas',
  imports: [CommonModule, ClaseComponent],
  standalone: true,
  templateUrl: './clasesInscritas.component.html',
  styleUrls: ['./clasesInscritas.component.css']
})

export class ClasesInscritasComponent{
  
  ListaClasesInscritas: ClaseDTO[] = [];

  constructor(private objetoService: ObjetoService, private authService: AuthService, private toastr: ToastrService  ){}
    
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
